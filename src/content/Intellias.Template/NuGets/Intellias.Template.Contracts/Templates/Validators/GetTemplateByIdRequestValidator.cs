namespace Intellias.Template.Contracts.Templates.Validators
{
    using FluentValidation;

    public class GetTemplateByIdRequestValidator : AbstractValidator<GetTemplateByIdRequest>
    {
        public GetTemplateByIdRequestValidator() => this.RuleFor(p => p.Id).NotEmpty();
    }
}
