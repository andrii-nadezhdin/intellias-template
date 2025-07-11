namespace Intellias.Template.Contracts.Templates.Validators
{
    using FluentValidation;

    public class UpdateRequestValidatorValidator : AbstractValidator<UpdateTemplateRequest>
    {
        public UpdateRequestValidatorValidator()
        {
            this.RuleFor(p => p.Id).NotEmpty();
            this.RuleFor(p => p.Name).NotEmpty();
            this.RuleFor(p => p.Description).NotEmpty();
            this.RuleFor(p => p.TemplateTypeId).NotEmpty();
        }
    }
}
