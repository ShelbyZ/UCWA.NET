using System;
using System.Runtime.Serialization;
using UCWA.NET.Resources;

namespace UCWA.NET.Core
{
    public class AuthenticationException : Exception
    {
        public Challenge Challenge { get; private set; }

        public AuthenticationException(Challenge challenge)
        {
            Challenge = challenge;
        }

        public AuthenticationException(Challenge challenge, string message) : base(message)
        {
            Challenge = challenge;
        }

        public AuthenticationException(Challenge challenge, string message, Exception innerException) : base(message, innerException)
        {
            Challenge = challenge;
        }

        protected AuthenticationException(Challenge challenge, SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Challenge = challenge;
        }
    }
}
