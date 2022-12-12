using MyHostAPI.Common.Enums;

namespace MyHostAPI.Common.Exceptions
{
    public class UniqueException : Exception
    {
        public UniqueException()
        {
            Source = ErrorSource.Conflict.ToString();
        }

        public UniqueException(string message) : base(message)
        {
            Source = ErrorSource.Conflict.ToString();
        }

        public UniqueException(string message, Exception innerException) : base(message, innerException)
        {
            Source = ErrorSource.Conflict.ToString();
        }
    }
}
