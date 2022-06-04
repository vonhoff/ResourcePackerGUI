#region GNU General Public License

/* Copyright 2022 Simon Vonhoff
 *
 * This file is part of ResourcePackerGUI.
 *
 * ResourcePackerGUI is free software: you can redistribute it and/or modify it under the terms of the
 * GNU General Public License as published by the Free Software Foundation, either version 3 of the License,
 * or (at your option) any later version.
 *
 * ResourcePackerGUI is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License along with ResourcePackerGUI.
 * If not, see <https://www.gnu.org/licenses/>.
 */

#endregion

using System.Text;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Force.Crc32;
using HeyRed.Mime;
using ResourcePacker.Entities;
using Serilog;
using Winista.Mime;

namespace ResourcePacker.Helpers
{
    internal static class AssetHelper
    {
        private static readonly MimeType JsonMimeType =
            new("application", "json")
            {
                Description = "JavaScript Object Notation"
            };

        private static readonly Lazy<MimeTypes> MimeTypes = new(() => new MimeTypes());

        /// <summary>
        /// Attempts to load all assets from a collection of entries.
        /// </summary>
        /// <param name="entries"></param>
        /// <param name="fileStream"></param>
        /// <param name="password"></param>
        /// <returns>A list of all assets inside the package.</returns>
        public static List<Asset> LoadAllFromPackage(Entry[] entries, Stream fileStream, string password = "")
        {
            var assets = new List<Asset>();
            foreach (var entry in entries)
            {
                if (!LoadSingleFromPackage(fileStream, entry, out var asset, password))
                {
                    Log.Error("Integrity check failed for entry: {id}", new { entry.Id });
                    continue;
                }

                Log.Debug("Added asset: {@asset}",
                    new { asset.Name, MediaType = asset.MimeType?.Name });
                assets.Add(asset);
            }

            if (assets.Count == entries.Length)
            {
                Log.Information("Loaded {assetCount} assets.", assets.Count);
            }
            else
            {
                Log.Warning("Loaded {assetCount} out of {entryCount} assets.",
                    assets.Count, entries.Length);
            }

            return assets;
        }

        /// <summary>
        /// Attempts to load a specified asset from a provided stream.
        /// </summary>
        /// <param name="fileStream">The package stream.</param>
        /// <param name="entry">Information about the entry.</param>
        /// <param name="asset"></param>
        /// <returns><see langword="true"/> when integrity check succeeded; otherwise, <see langword="false"/>.</returns>
        public static bool LoadSingleFromPackage(Stream fileStream, Entry entry, out Asset asset, string password = "")
        {
            var binaryReader = new BinaryReader(fileStream);
            binaryReader.BaseStream.Position = entry.Offset;
            var buffer = binaryReader.ReadBytes(entry.PackSize);

            if (!string.IsNullOrEmpty(password))
            {
                var key = AesEncryptionHelper.KeySetup(password);
                var output = new byte[entry.PackSize];
                AesEncryptionHelper.DecryptCbc(buffer, entry.PackSize, ref output, key);
                buffer = output;
            }

            var crc = Crc32Algorithm.Compute(buffer, 0, entry.DataSize);
            asset = new Asset(buffer);

            if (entry.Crc != crc)
            {
                return false;
            }

            asset.MimeType = GetMimeType(buffer);
            asset.Entry = entry;
            return true;
        }

        /// <summary>
        /// Updates the names of the provided assets from a dictionary of definitions.
        /// </summary>
        /// <param name="source">The collection of assets.</param>
        /// <param name="definitionDictionary">The dictionary of definitions.</param>
        /// <returns>The amount of matches.</returns>
        public static int UpdateAssetsWithDefinitions(IReadOnlyList<Asset> source, IReadOnlyDictionary<uint, string> definitionDictionary)
        {
            if (definitionDictionary.Count == 0 || source.Count == 0)
            {
                Log.Error("Could not update assets with definitions: {@info}",
                    new { AssetCount = source.Count, DefinitionCount = definitionDictionary.Count });
                return -1;
            }

            var matches = 0;

            foreach (var asset in source)
            {
                if (definitionDictionary.TryGetValue(asset.Entry.Id, out var filePath))
                {
                    // If the media type has not been found before,
                    // try to find the media type by the file extension.
                    if (asset.MimeType == null)
                    {
                        var typeMap = MimeTypesMap.GetMimeType(filePath);
                        if (typeMap != null && typeMap != "application/octet-stream")
                        {
                            asset.MimeType = MimeTypes.Value.ForName(typeMap);
                        }
                    }

                    asset.Name = filePath;
                    matches++;
                }
                else
                {
                    Log.Warning("Could not find definition for hash: {id}", asset.Entry.Id);
                    continue;
                }

                Log.Debug("Updated asset: {@asset}",
                    new { asset.Entry.Id, asset.Name });
            }

            if (matches == source.Count)
            {
                Log.Information("Updated {matchCount} asset names.", matches);
            }
            else
            {
                Log.Warning("Updated {matchCount} out of {expectedCount} asset names.",
                    matches, source.Count);
            }

            return matches;
        }

        private static MimeType? GetMimeType(byte[] buffer)
        {
            var mimeType = MimeTypes.Value.GetMimeType(buffer);

            if (mimeType != null)
            {
                return mimeType;
            }

            // Check that the current stream of bytes is a JSON type,
            // since JSON types are not automatically detected.
            try
            {
                var text = Encoding.UTF8.GetString(buffer);
                text = Regex.Replace(text, @"[^\t\r\n -~]", string.Empty, RegexOptions.Compiled);
                JsonNode.Parse(text);
                return JsonMimeType;
            }
            catch
            {
                return null;
            }
        }
    }
}