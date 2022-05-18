using Force.Crc32;
using ResourcePacker.Entities;
using Serilog;
using Winista.Mime;

namespace ResourcePacker.Helpers
{
    internal static class AssetHelper
    {
        private static readonly Lazy<MimeTypes> MimeTypes = new(() => new MimeTypes());

        /// <summary>
        /// Attempts to load a specified asset from a provided stream.
        /// </summary>
        /// <param name="fileStream">The package stream.</param>
        /// <param name="entry">Information about the entry.</param>
        /// <param name="asset"></param>
        /// <returns><see langword="true"/> when integrity check succeeded; otherwise, <see langword="false"/>.</returns>
        public static bool LoadAsset(Stream fileStream, Entry entry, out Asset asset)
        {
            var binaryReader = new BinaryReader(fileStream);
            binaryReader.BaseStream.Position = entry.Offset;
            var buffer = binaryReader.ReadBytes((int)(entry.DataSize + 1));

            var crc = Crc32Algorithm.Compute(buffer, 0, (int)entry.DataSize);
            asset = new Asset();

            if (entry.Crc != crc)
            {
                return false;
            }

            asset.MimeType = MimeTypes.Value.GetMimeType(buffer);
            asset.Data = buffer;
            asset.Entry = entry;
            return true;
        }

        public static List<Asset> LoadAssetsFromPackage(Pack package)
        {
            var assets = new List<Asset>();
            foreach (var entry in package.Entries)
            {
                if (!LoadAsset(package.FileStream, entry, out var asset))
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

        public static List<Asset> LoadAssetsFromPackage(Pack package, IReadOnlyDictionary<uint, string> crcDefinitionDictionary)
        {
            if (crcDefinitionDictionary.Count == 0)
            {
                return new List<Asset>();
            }

            var assets = new List<Asset>();
            var matches = 0;

            foreach (var entry in package.Entries)
            {
                if (!LoadAsset(package.FileStream, entry, out var asset))
                {
                    Log.Error("Integrity check failed for entry: {id}", new { entry.Id });
                    continue;
                }

                if (crcDefinitionDictionary.TryGetValue(entry.Id, out var filePath))
                {
                    asset.Name = filePath;
                    matches++;
                }
                else
                {
                    Log.Warning("Could not find definition for entry: {@id}", new { entry.Id });
                }

                Log.Debug("Added asset: {@asset}",
                    new { entry.Id, asset.Name, MediaType = asset.MimeType?.Name });
                assets.Add(asset);
            }

            Log.Information("Found {matchCount} out of {expectedCount} definitions.",
                matches, package.Entries.Length);

            return assets;
        }
    }
}