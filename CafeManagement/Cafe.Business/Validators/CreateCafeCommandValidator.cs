using Cafe.Business.Commands.Cafe;
using FluentValidation;

namespace Cafe.Business.Validators
{
    public class CreateCafeCommandValidator : AbstractValidator<CreateCafeCommand>
    {
        public CreateCafeCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(6, 10)
                .WithMessage("Cafe name must be between 6 and 10 characters");

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(256)
                .WithMessage("Description cannot exceed 256 characters");

            RuleFor(x => x.Location)
                .NotEmpty()
                .WithMessage("Location is required");
        }
    }
}
