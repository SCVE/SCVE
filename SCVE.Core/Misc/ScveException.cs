using System;

namespace SCVE.Core.Misc
{
    public class ScveException : Exception
    {
        public ScveException(string message) : base(message)
        {
        }
    }
}