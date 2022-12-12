using MyHostAPI.Common.Enums;

namespace MyHostAPI.Common.Exceptions
{
    public class SamePasswordException : Exception
    {
        public SamePasswordException()
        {
            Source = ErrorSource.Conflict.ToString();
        }

        public SamePasswordException(string message) : base(message)
        {
            Source = ErrorSource.Conflict.ToString();
        }

        public SamePasswordException(string message, Exception innerException) : base(message, innerException)
        {
            Source = ErrorSource.Conflict.ToString();
        }
    }
}
