using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ResourcePackerGUI.Application.Common.Exceptions
{
    [Serializable]
    public class InvalidPasswordException : Exception
    {
        public InvalidPasswordException()
        {
        }

        public InvalidPasswordException(string message)
            : base(message)
        {
        }

        public InvalidPasswordException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public InvalidPasswordException(string message, int passwordHash)
            : base(message)
        {
            PasswordHash = passwordHash;
        }

        public InvalidPasswordException(string message, int passwordHash, Exception innerException)
            : base(message, innerException)
        {
            PasswordHash = passwordHash;
        }

        protected InvalidPasswordException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            PasswordHash = info.GetInt32("PasswordHash");
        }

        public int PasswordHash { get; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            info.AddValue("PasswordHash", PasswordHash);
            base.GetObjectData(info, context);
        }
    }
}
