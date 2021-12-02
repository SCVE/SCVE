using System;

namespace SCVE.Engine.Core.Misc
{
    public class ScveException : Exception
    {
        public ScveException(string message) : base(message)
        {
        }
    }
}