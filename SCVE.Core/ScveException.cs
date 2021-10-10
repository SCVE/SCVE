using System;

namespace SCVE.Core
{
    public class ScveException : Exception
    {
        public ScveException(string message) : base(message)
        {
        }
    }
}