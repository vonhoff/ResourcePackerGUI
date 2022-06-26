using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ResourcePackerGUI.Application.Common.Exceptions
{
    [Serializable]
    public class InvalidOutputException : Exception
    {
        public InvalidOutputException()
        {
        }

        public InvalidOutputException(string message)
            : base(message)
        {
        }

        public InvalidOutputException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public InvalidOutputException(string message, string outputPath)
            : base(message)
        {
            OutputPath = outputPath;
        }

        public InvalidOutputException(string message, string outputPath, Exception innerException)
            : base(message, innerException)
        {
            OutputPath = outputPath;
        }

        protected InvalidOutputException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            OutputPath = info.GetString("OutputPath");
        }

        public string? OutputPath { get; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            info.AddValue("OutputPath", OutputPath);
            base.GetObjectData(info, context);
        }
    }
}
