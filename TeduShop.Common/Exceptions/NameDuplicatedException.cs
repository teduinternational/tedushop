using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeduShop.Common.Exceptions
{
    public class NameDuplicatedException : Exception
    {
        public NameDuplicatedException()
        {
        }

        public NameDuplicatedException(string message)
        : base(message)
        {
        }

        public NameDuplicatedException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
