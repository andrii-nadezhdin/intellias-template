namespace Intellias.Template.Contracts.Templates.Validators
{
    using FluentValidation;

    public class DeleteTemplateRequestValidator : AbstractValidator<DeleteTemplateRequest>
    {
        public DeleteTemplateRequestValidator() => this.RuleFor(p => p.Id).NotEmpty();
    }
}
