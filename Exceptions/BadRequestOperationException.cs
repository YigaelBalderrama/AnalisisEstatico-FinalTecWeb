using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Permissions;
using System.Runtime.Serialization;
namespace SimpsonApp.Exceptions
{
    [Serializable]
    public class BadRequestOperationException : Exception
    {
        protected BadRequestOperationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
        public BadRequestOperationException(string message) : base(message)
        {

        }
    }
}
