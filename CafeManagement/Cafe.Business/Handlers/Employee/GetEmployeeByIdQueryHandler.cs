using AutoMapper;
using Cafe.Business.DTOs;
using Cafe.Business.Queries.Cafe;
using Cafe.Data.Repositories.Interfaces;
using MediatR;

namespace Cafe.Business.Handlers.Employee
{
    public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeDto>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICafeEmployeeRepository _cafeEmployeeRepository;
        private readonly IMapper _mapper;

        public GetEmployeeByIdQueryHandler(IEmployeeRepository employeeRepository,
            ICafeEmployeeRepository cafeEmployeeRepository , IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _cafeEmployeeRepository = cafeEmployeeRepository;
            _mapper = mapper;
        }

        public async Task<EmployeeDto> Handle(GetEmployeeByIdQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var employee = await _employeeRepository.GetByIdAsync(query.Id);
                if (employee == null)
                    return null;

                var employeeDto = _mapper.Map<EmployeeDto>(employee);

                var cafeEmp = await _cafeEmployeeRepository.GetAllCafeEmployeeByEmployeeIdAsync(employeeDto.Id);
                if (cafeEmp != null)
                {
                    var mapCafeEmployees = _mapper.Map<IEnumerable<CafeEmployeeDto>>(cafeEmp);
                    
                    var activeCafeEmployee = mapCafeEmployees.Where(x => x.IsActive).FirstOrDefault();
                    employeeDto.CafeName = activeCafeEmployee != null ? activeCafeEmployee.CafeName : string.Empty;
                    employeeDto.CafeId = activeCafeEmployee != null ? activeCafeEmployee.Id : null;

                    employeeDto.DaysWorked = CalculateTotalDaysWorked(mapCafeEmployees);
                }
                return employeeDto;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving employee", ex);
            }
        }

        private int CalculateTotalDaysWorked(IEnumerable<CafeEmployeeDto> employments)
        {
            var totalDays = 0;
            foreach (var employment in employments)
            {
                var days = 0;
                if (employment.EndDate == default(DateTime))
                {
                    if (employment.IsActive)
                    {
                        days = (DateTime.Today - employment.StartDate).Days + 1;
                    }
                }
                else
                {
                    days = (employment.EndDate - employment.StartDate).Days + 1;
                }
                totalDays += days;
            }
            return totalDays;
        }
    }
}
