using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Force.Crc32;
using ResourcePacker.Entities;
using Serilog;
using Winista.Mime;

namespace ResourcePacker.Helpers
{
    internal static class AssetHelper
    {
        private static readonly Lazy<MimeTypes> MimeTypes = new(() => new MimeTypes());

        public static bool Load(Pack pack, Entry entry, out Asset asset)
        {
            var binaryReader = new BinaryReader(pack.FileStream);
            binaryReader.BaseStream.Position = entry.Offset;
            var buffer = binaryReader.ReadBytes((int)(entry.DataSize + 1));

            var crc = Crc32Algorithm.Compute(buffer, 0, (int)entry.DataSize);
            asset = new Asset();

            if (entry.Crc != crc)
            {
                return false;
            }

            asset.MimeType = GetMimeType(buffer);
            asset.Data = buffer;
            asset.Entry = entry;
            return true;
        }

        private static MimeType? GetMimeType(byte[] buffer)
        {
            return MimeTypes.Value.GetMimeType(buffer);
        }

        public static List<Asset> LoadAllFromPackage(Pack package)
        {
            var assets = new List<Asset>();
            foreach (var entry in package.Entries)
            {
                if (!Load(package, entry, out var asset))
                {
                    continue;
                }

                assets.Add(asset);
            }

            return assets;
        }

        public static List<Asset> LoadAllFromPackage(Pack package, IReadOnlyDictionary<uint, string> crcDefinitionDictionary)
        {
            if (crcDefinitionDictionary.Count == 0)
            {
                return new List<Asset>();
            }

            var assets = new List<Asset>();
            var matches = 0;

            foreach (var entry in package.Entries)
            {
                if (!Load(package, entry, out var asset))
                {
                    continue;
                }

                if (crcDefinitionDictionary.TryGetValue(entry.Id, out var filePath))
                {
                    Log.Debug("Found definition for entry: {@id}", new { entry.Id });
                    asset.Name = filePath;
                }
                else
                {
                    Log.Warning("Could not find definition for entry: {@id}", new { entry.Id });
                }

                assets.Add(asset);
                matches++;
            }

            Log.Information("Found {matchCount} out of {expectedCount} definitions.",
                matches, package.Entries.Length);

            return assets;
        }
    }
}
