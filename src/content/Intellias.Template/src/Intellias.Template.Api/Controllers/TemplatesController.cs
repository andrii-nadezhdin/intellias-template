namespace Intellias.Template.Api.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using Intellias.Template.Api.Constants;
    using Intellias.Template.Api.Responses;
    using Intellias.Template.Contracts.Templates;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    [ExcludeFromCodeCoverage]
    [ApiVersion(ApiVersions.V1)]
    public class TemplatesController : BaseController
    {
        public TemplatesController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Gets template by id.
        /// </summary>
        /// <param name="id">The id of template.</param>
        /// <returns>A <see cref="TemplateDTO"/> representing the template info.</returns>
        [HttpGet("{id:Guid}")]
        [MapToApiVersion(ApiVersions.V1)]
        [ProducesResponseType(typeof(ApiResponse<TemplateDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTemplateByIdAsync([FromRoute] Guid id)
        {
            var response = await this.Mediator.Send(new GetTemplateByIdRequest(id)).ConfigureAwait(false);
            return this.Ok(response);
        }

        /// <summary>
        /// Gets all templates by id.
        /// </summary>
        /// <returns>An array of <see cref="TemplateDTO"/> representing the templates info.</returns>
        [HttpGet]
        [MapToApiVersion(ApiVersions.V1)]
        [ProducesResponseType(typeof(ApiResponse<TemplateDTO[]>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTemplatesAsync()
        {
            var response = await this.Mediator.Send(new GetAllTemplatesRequest()).ConfigureAwait(false);
            return this.Ok(response);
        }

        /// <summary>
        /// Creates template.
        /// </summary>
        /// <param name="request">Templates info.</param>
        /// <returns>Returns id of created template.</returns>
        [HttpPost]
        [MapToApiVersion(ApiVersions.V1)]
        [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTemplateAsync(CreateTemplateRequest request)
        {
            var response = await this.Mediator.Send(request).ConfigureAwait(false);
            return this.Ok(response);
        }

        /// <summary>
        /// Updates template.
        /// </summary>
        /// <param name="request">Templates info.</param>
        /// <returns>Returns id of updates template.</returns>
        [HttpPut]
        [MapToApiVersion(ApiVersions.V1)]
        [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateTemplateAsync(UpdateTemplateRequest request)
        {
            var response = await this.Mediator.Send(request).ConfigureAwait(false);
            return this.Accepted(response);
        }

        /// <summary>
        /// Deletes template.
        /// </summary>
        /// <param name="id">The id of template.</param>
        /// <returns>Returns id of deleted template.</returns>
        [HttpDelete("{id:Guid}")]
        [MapToApiVersion(ApiVersions.V1)]
        [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteTemplateAsync(Guid id)
        {
            var response = await this.Mediator.Send(new DeleteTemplateRequest(id)).ConfigureAwait(false);
            return this.Ok(response);
        }
    }
}
