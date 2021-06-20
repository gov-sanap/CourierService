using System;
using System.Runtime.Serialization;

namespace CourierService.Exceptions
{
    [Serializable]
    public class NegativeDiscountException : Exception
    {
        public NegativeDiscountException()
        {
        }

        public NegativeDiscountException(string message) : base(message)
        {
        }

        public NegativeDiscountException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NegativeDiscountException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}