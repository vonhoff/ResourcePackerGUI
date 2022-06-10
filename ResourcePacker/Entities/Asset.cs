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

using Winista.Mime;

namespace ResourcePacker.Entities
{
    public class Asset
    {
        private string _name = string.Empty;

        public Asset(byte[] data)
        {
            Data = data;
        }

        public Asset()
        {
            Data = Array.Empty<byte>();
        }

        public byte[] Data { get; init; }

        public Entry Entry { get; set; }

        public MimeType? MimeType { get; set; }

        public string Name
        {
            get
            {
                if (!string.IsNullOrEmpty(_name))
                {
                    return _name;
                }

                var name = Entry.Id.ToString();
                var extension = MimeType?.Extensions.FirstOrDefault();

                if (extension == null && !string.IsNullOrWhiteSpace(MimeType?.SubType))
                {
                    extension = MimeType.SubType;
                }

                if (extension != null)
                {
                    name += $".{extension}";
                }

                return name;
            }
            set => _name = value;
        }
    }
}