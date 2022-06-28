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