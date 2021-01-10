using System;

namespace Cw10.Exceptions
{
    public class SqlInsertException : Exception
    {
        public SqlInsertException() { }
        public SqlInsertException(string message) : base(message) { }
    }
}