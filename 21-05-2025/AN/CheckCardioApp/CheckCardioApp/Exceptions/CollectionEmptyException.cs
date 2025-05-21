using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckCardioApp.Exceptions
{
    public class CollectionEmptyException : Exception
    {
        public CollectionEmptyException(string message) : base(message) { }
    }
}
