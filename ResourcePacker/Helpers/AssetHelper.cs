#region GNU General Public License

/* Copyright 2022 Vonhoff, MaxtorCoder
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
using HeyRed.Mime;
using ResourcePacker.Entities;
using ResourcePacker.Forms;
using Serilog;
using Winista.Mime;

namespace ResourcePacker.Helpers
{
    internal static class AssetHelper
    {
        private static readonly Regex TextRegex = new(@"[^\t\r\n -~]", RegexOptions.Compiled);

        private static readonly MimeType JsonMimeType =
            new("application", "json")
            {
                Description = "JavaScript Object Notation"
            };

        private static readonly Lazy<MimeTypes> MimeTypes = new(() => new MimeTypes());

        public static MimeType? GetMimeType(byte[] buffer)
        {
            if (buffer.Length == 0)
            {
                return null;
            }

            var mimeType = MimeTypes.Value.GetMimeType(buffer);
            if (mimeType != null)
            {
                return mimeType;
            }

            // Check that the current stream of bytes is a JSON type,
            // since JSON types are not automatically detected.
            try
            {
                // Return null when the buffer does not start with a left curly bracket.
                if (buffer[0] != 0x7B)
                {
                    return null;
                }

                var text = Encoding.UTF8.GetString(buffer);
                text = TextRegex.Replace(text, string.Empty);
                JsonNode.Parse(text);
                return JsonMimeType;
            }
            catch
            {
                return null;
            }
        }

        public static int ExtractAssetsToLocation(IReadOnlyList<Asset> assets, string basePath,
            IProgress<(int percentage, int amount)>? progress = null, int progressReportInterval = 100,
            CancellationToken cancellationToken = default)
        {
            if (!Path.EndsInDirectorySeparator(basePath))
            {
                basePath += Path.DirectorySeparatorChar;
            }

            var duplicates = assets.Count(asset => File.Exists(basePath + asset.Name));

            if (duplicates > 0)
            {
                var a = duplicates == 1 ? "conflict" : "conflicts";
                Log.Warning("{amount} file " + a + " found.", duplicates);
            }

            using var progressTimer = new System.Timers.Timer(progressReportInterval);
            var percentage = 0;
            var i = 0;
            var extracted = 0;
            var failed = 0;
            var ignored = 0;
            DialogResult? resultForAllCases = null;
            progressTimer.Elapsed += delegate { progress!.Report((percentage, i + 1)); };
            progressTimer.Enabled = progress != null;

            for (; i < assets.Count; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var asset = assets[i];

                FileInfo fileInfo;
                try
                {
                    fileInfo = new FileInfo(basePath + asset.Name);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Could not get file information from: {path}", basePath + asset.Name);
                    continue;
                }

                if (fileInfo.Exists)
                {
                    var availableFileName = FilenameHelper.NextAvailableFilename(fileInfo.FullName);
                    var replaceDialog = resultForAllCases == null
                        ? new ReplaceDialog(fileInfo.FullName, availableFileName, duplicates--)
                        : null;

                    var dialogResult = resultForAllCases ?? replaceDialog!.ShowDialog();
                    if (dialogResult == DialogResult.Continue)
                    {
                        fileInfo = new FileInfo(availableFileName);
                    }
                    else if (dialogResult != DialogResult.OK && dialogResult != DialogResult.Ignore)
                    {
                        throw new OperationCanceledException();
                    }

                    if (replaceDialog is { UseForAllCases: true })
                    {
                        resultForAllCases = dialogResult;
                    }

                    if (dialogResult == DialogResult.Ignore)
                    {
                        Log.Debug("Ignored: {name}", Path.GetFileName(asset.Name));
                        ignored++;
                        continue;
                    }
                }

                fileInfo.Directory?.Create();

                try
                {
                    var file = File.OpenWrite(fileInfo.FullName);
                    using var binaryWriter = new BinaryWriter(file);
                    binaryWriter.Write(asset.Data);
                    binaryWriter.Flush();
                    binaryWriter.Close();
                    Log.Debug("Extracted {name} to: {path}",
                        Path.GetFileName(asset.Name), fileInfo.FullName);
                    extracted++;
                }
                catch (Exception ex)
                {
                    failed++;
                    Log.Error(ex, "Could not extract asset: {path}", fileInfo.FullName);
                }

                percentage = (int)((double)(i + 1) / assets.Count * 100);
            }

            progress?.Report((100, assets.Count));

            if (extracted == assets.Count)
            {
                Log.Information("Extracted all {amount} files to: {basePath}",
                    extracted, basePath);
            }
            else
            {
                Log.Information("Extracted {amount} out of {expected} files to: {basePath}",
                    extracted, assets.Count, basePath);

                var a = ignored == 1 ? "asset" : "assets";
                var b = failed == 1 ? "asset" : "assets";
                Log.Information("{ignored} " + a + " ignored for extraction. " +
                                "{failed} " + b + " failed to extract.",
                    ignored, failed);
            }

            return extracted;
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
                Log.Information("Updated all {matchCount} asset names.", matches);
            }
            else
            {
                Log.Warning("Updated {matchCount} out of {expectedCount} asset names.",
                    matches, source.Count);
            }

            return matches;
        }
    }
}