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

using ResourcePackerGUI.Domain.Structures;

namespace ResourcePackerGUI.Domain.Entities
{
    public class Package
    {
        public Package(PackageHeader header, IReadOnlyList<Entry> entries, bool encrypted)
        {
            Encrypted = encrypted;
            Header = header;
            Entries = entries;
        }

        public IReadOnlyList<Entry> Entries { get; init; }

        public bool Encrypted { get; set; }

        public PackageHeader Header { get; init; }
    }
}