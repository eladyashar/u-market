using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace u_market.Exceptions
{
    public class ModelValidationException : Exception
    {
        public ModelValidationException(string message) : base(message)
        {

        }
    }
}