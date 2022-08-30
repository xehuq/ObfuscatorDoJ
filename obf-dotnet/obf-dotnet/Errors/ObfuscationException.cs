using System;
using System.Collections.Generic;
using System.Text;

namespace Obfuscator.Errors
{
    public class ObfuscationException : Exception
    {
        public ObfuscationException()
        {
        }

        public ObfuscationException(string message) : base(message)
        {
        }
    }
}