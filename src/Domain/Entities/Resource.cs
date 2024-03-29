﻿#region GNU General Public License

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

using ResourcePackerGUI.Domain.Structures;

namespace ResourcePackerGUI.Domain.Entities
{
    public class Resource
    {
        private string _name;

        public Resource(byte[] data, Entry entry, MediaType? mediaType = null, string name = "")
        {
            Data = data;
            Entry = entry;
            MediaType = mediaType;
            _name = name;
        }

        public byte[] Data { get; init; }
        public Entry Entry { get; init; }
        public MediaType? MediaType { get; set; }

        public string Name
        {
            get
            {
                if (!string.IsNullOrEmpty(_name))
                {
                    return _name;
                }

                var name = Entry.Id.ToString();
                var extension = MediaType?.Extensions.FirstOrDefault();
                if (!string.IsNullOrEmpty(extension))
                {
                    name += $".{extension}";
                }

                return name;
            }
            set => _name = value;
        }

        public bool NameDefined => !string.IsNullOrEmpty(_name);
    }
}