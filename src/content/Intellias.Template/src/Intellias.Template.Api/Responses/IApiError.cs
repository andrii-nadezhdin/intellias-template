namespace Intellias.Template.Api.Responses
{
    public interface IApiError
    {
        int ErrorCode { get; }

        string? ErrorMessage{ get; }
    }
}
