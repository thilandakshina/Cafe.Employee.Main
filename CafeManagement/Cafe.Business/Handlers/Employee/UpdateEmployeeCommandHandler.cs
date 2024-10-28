using AutoMapper;
using Cafe.Business.Commands.Employee;
using Cafe.Business.DTOs;
using Cafe.Data.Repositories.Interfaces;
using MediatR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Cafe.Business.Handlers.Employee
{
    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, bool>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICafeEmployeeRepository _cafeEmployeeRepository;
        private readonly ICafeRepository _cafeRepository;
        private readonly IMapper _mapper;

        public UpdateEmployeeCommandHandler(IEmployeeRepository employeeRepository,
            IMapper mapper, ICafeEmployeeRepository cafeEmployeeRepository , ICafeRepository cafeRepository)
        {
            _employeeRepository = employeeRepository;
            _cafeEmployeeRepository = cafeEmployeeRepository;
            _cafeRepository = cafeRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateEmployeeCommand command, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(command.Name) || command.Name.Length < 6 || command.Name.Length > 10)
                throw new ArgumentException("Name must be between 6 and 10 characters");

            var existingEmployee = await _employeeRepository.GetByIdAsync(command.Id);
            if (existingEmployee == null)
                throw new ArgumentException($"Employee with ID {command.Id} not found");

            var empCafeAssignmentInfo = await _cafeEmployeeRepository.GetCurrentEmploymentAsync(command.Id);
            if(empCafeAssignmentInfo != null && !command.CafeId.HasValue)
            {
                    await _cafeEmployeeRepository.UnassignEmployeeFromCafeAsync(command.Id);
            }
            else if(command.CafeId.HasValue)
            {
                var cafe = await _cafeRepository.GetCafeByIdAsync(command.CafeId.Value);
                if(cafe != null)
                {
                    var cafeDtos = _mapper.Map<CafeDto>(cafe);
                    await _cafeEmployeeRepository.AssignEmployeeToCafeAsync(command.Id, command.Name, command.CafeId.Value, cafeDtos.Name);
                }
            }

            _mapper.Map(command, existingEmployee);
            await _employeeRepository.UpdateAsync(existingEmployee);
            return true;
        }
    }
}
