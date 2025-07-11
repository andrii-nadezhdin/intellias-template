namespace Intellias.Template.Contracts.Templates.Validators
{
    using FluentValidation;

    public class CreateTemplateRequestValidator : AbstractValidator<CreateTemplateRequest>
    {
        public CreateTemplateRequestValidator()
        {
            this.RuleFor(p => p.Name).NotEmpty();
            this.RuleFor(p => p.Description).NotEmpty();
            this.RuleFor(p => p.TemplateTypeId).NotEmpty();
        }
    }
}
