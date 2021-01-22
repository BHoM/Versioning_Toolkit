using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Upgrader.Base
{
    class NoUpdateException : Exception
    {
        public NoUpdateException(string message) : base(message) {}
    }
}
