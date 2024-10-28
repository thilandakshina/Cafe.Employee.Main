using MediatR;
using static Cafe.Business.Common;

namespace Cafe.Business.Commands.Employee
{
    public record CreateEmployeeCommand : IRequest<string>
    {
        public string Name { get; init; }
        public string EmailAddress { get; init; }
        public string PhoneNumber { get; init; }
        public GenderType Gender { get; set; } = GenderType.Male;
        public Guid? CafeId { get; init; }
    }
}
