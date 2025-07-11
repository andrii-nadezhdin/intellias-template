namespace Intellias.Template.Api.Responses
{
    public class ApiError : IApiError, IApiResponse
    {
        public ApiError(int errorCode, string? errorMessage = null)
        {
            this.ErrorCode = errorCode;
            this.ErrorMessage = errorMessage;
        }

        public int ErrorCode { get; private set; }

        public string? ErrorMessage { get; private set; }

        public bool IsSuccess => false;
    }
}
