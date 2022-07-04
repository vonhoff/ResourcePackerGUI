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

using System.Runtime.Serialization;

namespace ResourcePackerGUI.Application.Common.Exceptions
{
    [Serializable]
    public class InvalidPathException : Exception
    {
        public InvalidPathException()
        {
        }

        public InvalidPathException(string message)
            : base(message)
        {
        }

        public InvalidPathException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public InvalidPathException(string message, string path)
            : base(message)
        {
            Path = path;
        }

        public InvalidPathException(string message, string path, Exception innerException)
            : base(message, innerException)
        {
            Path = path;
        }

        protected InvalidPathException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Path = info.GetString("Path");
        }

        public string? Path { get; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            info.AddValue("Path", Path);
            base.GetObjectData(info, context);
        }
    }
}