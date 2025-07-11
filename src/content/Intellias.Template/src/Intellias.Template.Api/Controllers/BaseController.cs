namespace Intellias.Template.Api.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using Intellias.Template.Api.Responses;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    [ExcludeFromCodeCoverage]
    [ApiController]
    //[Authorize]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public abstract class BaseController : ControllerBase
    {
        public BaseController(IMediator mediator) => this.Mediator = mediator;

        protected IMediator Mediator { get; private set; }

        public OkObjectResult Ok<TResponse>(TResponse value) => base.Ok(new ApiResponse<TResponse>(value));

        public AcceptedResult Accepted<TResponse>(TResponse value) => base.Accepted(new ApiResponse<TResponse>(value));
    }
}
