using AutoMapper;
using Cafe.Business.DTOs;
using Cafe.Business.Queries.Employee;
using Cafe.Data.Entities;
using Cafe.Data.Repositories.Interfaces;
using MediatR;

namespace Cafe.Business.Handlers.Employee
{
    public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, IEnumerable<EmployeeDto>>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICafeRepository _cafeRepository;
        private readonly ICafeEmployeeRepository _cafeEmployeeRepository;
        private readonly IMapper _mapper;

        public GetEmployeesQueryHandler(IEmployeeRepository employeeRepository,
            ICafeRepository cafeRepository, ICafeEmployeeRepository cafeEmployeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _cafeRepository = cafeRepository;
            _cafeEmployeeRepository = cafeEmployeeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EmployeeDto>> Handle(GetEmployeesQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var employees = query.CafeId.HasValue
                    ? await _employeeRepository.GetByCafeIdAsync(query.CafeId.Value)
                    : await _employeeRepository.GetAllAsync();

                var employeeDto = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

                var cafeEmployees = await _cafeEmployeeRepository.GetAllAsync();
                var cafeEmployeeDto = _mapper.Map<IEnumerable<CafeEmployeeDto>>(cafeEmployees);

                if (cafeEmployeeDto.Any())
                {
                    foreach(var emp in employeeDto)
                    {
                        var activeCafeEmp = cafeEmployeeDto.Where(x => x.EmployeeId == emp.Id && x.IsActive).FirstOrDefault();
                        
                        if(activeCafeEmp != null)
                        {
                            emp.CafeName = activeCafeEmp.CafeName;
                            emp.CafeId = activeCafeEmp.Id;

                        }
                        var allCafeEmp = cafeEmployeeDto.Where(x => x.EmployeeId == emp.Id).ToList();
                        emp.DaysWorked = CalculateTotalDaysWorked(allCafeEmp);
                    }
                }
                return employeeDto.OrderByDescending(x => x.DaysWorked);

            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving employees", ex);
            }
        }

        private int CalculateTotalDaysWorked(List<CafeEmployeeDto> employments)
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
