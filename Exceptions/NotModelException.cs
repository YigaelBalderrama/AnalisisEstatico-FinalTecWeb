using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SimpsonApp.Exceptions
{
    [Serializable]
    public class NotModelException : Exception
    {
        protected NotModelException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
        public NotModelException(string message)
            :base(message)
        {

        }
    }
}
