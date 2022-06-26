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