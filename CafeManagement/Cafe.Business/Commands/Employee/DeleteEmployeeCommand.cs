using MediatR;

namespace Cafe.Business.Commands.Employee
{
    public class DeleteEmployeeCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
