using MyHostAPI.Common.Enums;

namespace MyHostAPI.Common.Exceptions
{
    public class RecordNotFoundException : Exception
    {
        public RecordNotFoundException()
        {
            Source = ErrorSource.Database.ToString();
        }

        public RecordNotFoundException(string message) : base(message)
        {
            Source = ErrorSource.Database.ToString();
        }

        public RecordNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
            Source = ErrorSource.Database.ToString();
        }
    }
}
