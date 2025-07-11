namespace Intellias.Template.Api.Responses
{
    public class ApiResponse<TResponse> : IApiResponse
    {
        public ApiResponse(TResponse data) => this.Data = data;

        public TResponse Data { get; private set; }

        public bool IsSuccess => true;
    }
}
