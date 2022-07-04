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
    public class InvalidHeaderException : Exception
    {
        public InvalidHeaderException()
        {
        }

        public InvalidHeaderException(string message)
            : base(message)
        {
        }

        public InvalidHeaderException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public InvalidHeaderException(string message, ulong invalidHeader)
            : base(message)
        {
            InvalidHeader = invalidHeader;
        }

        public InvalidHeaderException(string message, ulong invalidHeader, Exception innerException)
            : base(message, innerException)
        {
            InvalidHeader = invalidHeader;
        }

        protected InvalidHeaderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            InvalidHeader = info.GetUInt64("InvalidHeader");
        }

        public ulong InvalidHeader { get; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            info.AddValue("InvalidHeader", InvalidHeader);
            base.GetObjectData(info, context);
        }
    }
}