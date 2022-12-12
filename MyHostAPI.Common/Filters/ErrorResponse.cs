using MyHostAPI.Common.Enums;

namespace MyHostAPI.Common.Filters
{
    public class ErrorResponse
    {
        public string Message { get; set; }

        public string Type { get; set; }

        public string Path { get; set; }

        public string StackTrace { get; set; }

        public int Code { get; set; }


        public ErrorResponse()
        {
            Code = 500;
            Type = ErrorSource.Application.ToString();
        }
    }
}
