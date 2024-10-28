using Cafe.Business.Commands.Employee;
using FluentValidation;
using static Cafe.Business.Common;

namespace Cafe.Business.Validators
{
    public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
    {
        public CreateEmployeeCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(6, 10);
            RuleFor(x => x.EmailAddress).NotEmpty().EmailAddress();
            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .Matches(@"^[89]\d{7}$")
                .WithMessage("Phone number must start with 8 or 9 and have 8 digits");
            RuleFor(x => x.Gender).NotEmpty().Must(x => x == GenderType.Male
            || x == GenderType.Female || x == GenderType.Other);
        }
    }
}
