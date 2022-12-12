using MyHostAPI.Common.Enums;

namespace MyHostAPI.Common.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException()
        {
            Source = ErrorSource.Authorization.ToString();
        }

        public UnauthorizedException(string? message) : base(message)
        {
            Source = ErrorSource.Authorization.ToString();
        }

        public UnauthorizedException(string? message, Exception? innerException) : base(message, innerException)
        {
            Source = ErrorSource.Authorization.ToString();
        }
    }
}

