#region GNU General Public License

/* Copyright 2022 Simon Vonhoff & Contributors
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
using MimeTypes;
using ResourcePackerGUI.Application.Common.Interfaces;
using ResourcePackerGUI.Domain.Entities;

namespace ResourcePackerGUI.Infrastructure.Services
{
    public class MediaTypeService : IMediaTypeService
    {
        // The default value returned by the mapper when a mime type was not found.
        private const string InvalidMimeType = "application/octet-stream";

        // Values for the JSON type, as JSON types are not automatically detected.
        private static readonly MediaType JsonMediaType = new()
        {
            Description = "Javascript Object Notation",
            Extensions = new[] { "json" },
            PrimaryType = "application",
            SubType = "json"
        };

        private static readonly Winista.Mime.MimeTypes MimeTypes = new();

        public MediaType? GetTypeByData(byte[] data)
        {
            var mimeType = data.Length == 0 ? null : MimeTypes.GetMimeType(data);
            if (mimeType != null)
            {
                return new MediaType
                {
                    PrimaryType = mimeType.PrimaryType,
                    SubType = mimeType.SubType,
                    Description = mimeType.Description,
                    Extensions = mimeType.Extensions
                };
            }

            return !IsJsonText(data) ? null : JsonMediaType;
        }

        public MediaType? GetTypeByName(string name)
        {
            var mappedType = MimeTypeMap.GetMimeType(name);
            if (mappedType is null or InvalidMimeType)
            {
                return null;
            }

            if (name.Length >= 5 && name.Substring(name.Length - 5, 5) == ".json")
            {
                return JsonMediaType;
            }

            var mimeTypeFromMapped = MimeTypes.ForName(mappedType);
            if (mimeTypeFromMapped != null)
            {
                return new MediaType
                {
                    PrimaryType = mimeTypeFromMapped.PrimaryType,
                    SubType = mimeTypeFromMapped.SubType,
                    Description = mimeTypeFromMapped.Description,
                    Extensions = mimeTypeFromMapped.Extensions
                };
            }

            return null;
        }

        private static bool IsJsonText(byte[] data)
        {
            // Return false when the data does not start with a left curly bracket.
            if (data.Length == 0 || data[0] != 0x7B)
            {
                return false;
            }

            try
            {
                var text = Encoding.UTF8.GetString(data);
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