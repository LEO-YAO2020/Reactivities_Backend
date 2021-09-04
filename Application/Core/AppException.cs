namespace Application.Core
{
    public class AppException
    {
        private int StatusCode { get; set; }
        private string Message { get; set; }
        private string Details { get; set; }

        public AppException(int statusCode, string message, string details = null)
        {
            StatusCode = statusCode;
            Message = message;
            Details = details;
        }
    }
}