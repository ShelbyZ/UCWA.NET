using System;
using System.Runtime.Serialization;

namespace UCWA.NET.Core
{
    public class DiscoveryException : Exception
    {
        public DiscoveryException()
        {
        }

        public DiscoveryException(string message) : base(message)
        {
        }

        public DiscoveryException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DiscoveryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
