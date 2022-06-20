using System.Text;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using HeyRed.Mime;
using ResourcePackerGUI.Application.Common.Interfaces;
using ResourcePackerGUI.Domain.ValueObjects;
using Winista.Mime;

namespace ResourcePackerGUI.Infrastructure.Services
{
    public class MimeTypeService : IMimeTypeService
    {
        // The default value returned by the mapper from "HeyRed.Mime" when a mime type was not found.
        private const string InvalidMimeType = "application/octet-stream";

        // Values for the JSON type, as JSON types are not automatically detected.
        private static readonly MediaType JsonMediaType = new()
        {
            Description = "Javascript object notation",
            Extensions = new[] { "json" },
            PrimaryType = "application",
            SubType = "json"
        };

        private static readonly Regex InvalidCharactersRegex = new(@"[^\t\r\n -~]", RegexOptions.Compiled);
        private static readonly MimeTypes MimeTypes = new();

        public MediaType? GetTypeByData(byte[] data)
        {
            var mimeType = data.Length == 0 ? null : MimeTypes.GetMimeType(data);
            if (mimeType != null)
            {
                return FromMimeType(mimeType);
            }

            return !IsJsonText(data) ? null : JsonMediaType;
        }

        public MediaType? GetTypeByName(string name)
        {
            var mappedType = MimeTypesMap.GetMimeType(name);
            if (mappedType is null or InvalidMimeType)
            {
                return null;
            }

            var mimeTypeFromMapped = MimeTypes.ForName(mappedType);
            return mimeTypeFromMapped is null ? null :
                FromMimeType(mimeTypeFromMapped);
        }

        private static MediaType FromMimeType(MimeType mimeType)
        {
            return new MediaType
            {
                PrimaryType = mimeType.PrimaryType,
                SubType = mimeType.SubType,
                Description = mimeType.Description,
                Extensions = mimeType.Extensions
            };
        }

        private static bool IsJsonText(byte[] data)
        {
            // Return false when the data does not start with a left curly bracket.
            if (data[0] != 0x7B)
            {
                return false;
            }

            try
            {
                var text = Encoding.UTF8.GetString(data);
                text = InvalidCharactersRegex.Replace(text, string.Empty);
                JsonNode.Parse(text);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}