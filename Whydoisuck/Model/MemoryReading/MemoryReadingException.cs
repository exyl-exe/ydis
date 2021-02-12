using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whydoisuck.Model.MemoryReading
{
    public class MemoryReadingException : Exception
    {
        private const string ERROR_FORMAT = "An exception occured while trying to read memory : expected {0} bytes but read {1}";

        public MemoryReadingException(int expected, int obtained)
            : base(string.Format(ERROR_FORMAT, expected, obtained))
        {

        }
    }
}
