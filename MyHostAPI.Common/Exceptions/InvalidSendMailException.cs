using MyHostAPI.Common.Enums;

namespace MyHostAPI.Common.Exceptions
{
    public class InvalidSendMailException : Exception
    {
        public InvalidSendMailException()
        {
            Source = ErrorSource.Application.ToString();
        }

        public InvalidSendMailException(string message) : base(message)
        {
            Source = ErrorSource.Application.ToString();
        }

        public InvalidSendMailException(string message, Exception innerException) : base(message, innerException)
        {
            Source = ErrorSource.Application.ToString();
        }
    }
}
