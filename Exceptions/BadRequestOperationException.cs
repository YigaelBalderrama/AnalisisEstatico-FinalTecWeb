using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpsonApp.Exceptions
{
    public class BadRequestOperationException:Exception
    {
        public BadRequestOperationException(string message)
            :base(message)
        {

        }
    }
}
