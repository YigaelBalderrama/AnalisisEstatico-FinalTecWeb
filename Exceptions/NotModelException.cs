using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpsonApp.Exceptions
{
    public class NotModelException : Exception
    {
        public NotModelException(string message)
            :base(message)
        {

        }
    }
}
