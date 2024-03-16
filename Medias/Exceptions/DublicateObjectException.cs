using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medias.Exceptions
{
    public class DublicateObjectException : Exception
    {
        public DublicateObjectException() { }
        public DublicateObjectException(string message) : base(message) { }

    }
}
