using AutoMapper;
using Cafe.Business.Commands.Employee;
using Cafe.Data.Repositories.Interfaces;
using MediatR;

namespace Cafe.Business.Handlers.Employee
{
    public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, bool>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICafeEmployeeRepository _cafeEmployeeRepository;
        private readonly IMapper _mapper;

        public DeleteEmployeeCommandHandler(IEmployeeRepository employeeRepository ,
            ICafeEmployeeRepository cafeEmployeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _cafeEmployeeRepository = cafeEmployeeRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(DeleteEmployeeCommand command, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetByIdAsync(command.Id);
            if (employee == null)
                throw new ArgumentException($"Employee with ID {command.Id} not found");

            await _cafeEmployeeRepository.UnassignEmployeeFromCafeAsync(employee.Id);

            employee.IsActive = false;
            await _employeeRepository.UpdateAsync(employee);
            return true;
        }
    }
}
