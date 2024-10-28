using Cafe.Business.DTOs;
using MediatR;

namespace Cafe.Business.Queries.Cafe
{
    public record GetEmployeeByIdQuery : IRequest<EmployeeDto>
    {
        public Guid Id { get; init; }
    }
}
