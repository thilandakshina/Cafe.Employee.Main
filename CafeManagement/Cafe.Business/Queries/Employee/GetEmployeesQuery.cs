using Cafe.Business.DTOs;
using MediatR;

namespace Cafe.Business.Queries.Employee
{
    public record GetEmployeesQuery : IRequest<IEnumerable<EmployeeDto>>
    {
        public Guid? CafeId { get; init; }
    }
}
