using System;
using System.Runtime.Serialization;

namespace CourierService.Exceptions
{
    [Serializable]
    public class InvalidRequestException : Exception
    {
        public InvalidRequestException()
        {
        }

        public InvalidRequestException(string message) : base(message)
        {
        }

        public InvalidRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}