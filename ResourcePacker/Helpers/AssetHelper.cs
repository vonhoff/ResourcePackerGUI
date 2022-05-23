﻿using System.Text;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Force.Crc32;
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
        /// Attempts to load all assets inside a <see cref="Pack"/>.
        /// </summary>
        /// <param name="package">The package to load the assets from.</param>
        /// <returns>A list of all assets inside the <paramref name="package"/>.</returns>
        public static List<Asset> LoadAllFromPackage(Pack package)
        {
            var assets = new List<Asset>();
            foreach (var entry in package.Entries)
            {
                if (!LoadSingleFromPackage(package, entry, out var asset))
                {
                    Log.Error("Integrity check failed for entry: {id}", new { entry.Id });
                    continue;
                }

                Log.Debug("Added asset: {@asset}",
                    new { asset.Name, MediaType = asset.MimeType?.Name });
                assets.Add(asset);
            }

            return assets;
        }

        /// <summary>
        /// Attempts to load a specified asset from a provided stream.
        /// </summary>
        /// <param name="pack">The package.</param>
        /// <param name="entry">Information about the entry.</param>
        /// <param name="asset"></param>
        /// <returns><see langword="true"/> when integrity check succeeded; otherwise, <see langword="false"/>.</returns>
        public static bool LoadSingleFromPackage(Pack pack, Entry entry, out Asset asset)
        {
            var binaryReader = new BinaryReader(pack.FileStream);
            binaryReader.BaseStream.Position = entry.Offset;
            var buffer = binaryReader.ReadBytes((int)(entry.DataSize + 1));

            var crc = Crc32Algorithm.Compute(buffer, 0, (int)entry.DataSize);
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
                    new {AssetCount = source.Count, DefinitionCount = definitionDictionary.Count});
                return -1;
            }

            var matches = 0;

            foreach (var asset in source)
            {
                if (definitionDictionary.TryGetValue(asset.Entry.Id, out var filePath))
                {
                    asset.Name = filePath;
                    matches++;
                }
                else
                {
                    Log.Warning("Could not find definition for entry: {@id}", new { asset.Entry.Id });
                }

                Log.Debug("Updated asset: {@asset}",
                    new { asset.Entry.Id, asset.Name, MediaType = asset.MimeType?.Name });
            }

            Log.Information("Found {matchCount} out of {expectedCount} definitions.",
                matches, source.Count);

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