using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SimpsonApp.Exceptions
{
    [Serializable]
    public class DatabaseException : Exception
    {
        protected DatabaseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
        public DatabaseException(string message)
            : base(message)
        {

        }
    }
}