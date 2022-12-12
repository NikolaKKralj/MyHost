using MyHostAPI.Common.Enums;

namespace MyHostAPI.Common.Exceptions
{
    public class InvalidLoginException : Exception
    {
        public InvalidLoginException()
        {
            Source = ErrorSource.Authentication.ToString();
        }

        public InvalidLoginException(string message) : base(message)
        {
            Source = ErrorSource.Authentication.ToString();
        }

        public InvalidLoginException(string message, Exception innerException) : base(message, innerException)
        {
            Source = ErrorSource.Authentication.ToString();
        }
    }
}
