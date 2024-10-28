using AutoMapper;
using Cafe.Business.Commands.Employee;
using Cafe.Data.Entities;
using Cafe.Data.Repositories.Interfaces;
using MediatR;

namespace Cafe.Business.Handlers.Employee
{
    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, string>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICafeRepository _cafeRepository;
        private readonly IMapper _mapper;

        public CreateEmployeeCommandHandler(
            IEmployeeRepository employeeRepository,
            ICafeRepository cafeRepository,
            IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _cafeRepository = cafeRepository;
            _mapper = mapper;
        }

        public async Task<string> Handle(CreateEmployeeCommand command, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(command.Name) || command.Name.Length < 6 || command.Name.Length > 10)
                throw new ArgumentException("Name must be between 6 and 10 characters");

            if (string.IsNullOrEmpty(command.EmailAddress) || !IsValidEmail(command.EmailAddress))
                throw new ArgumentException("Valid email address is required");

            if (string.IsNullOrEmpty(command.PhoneNumber) || !IsValidPhoneNumber(command.PhoneNumber))
                throw new ArgumentException("Phone number must start with 8 or 9 and have 8 digits");

            if (command.CafeId.HasValue)
            {
                var cafe = await _cafeRepository.GetByIdAsync(command.CafeId.Value);
                if (cafe == null)
                    throw new ArgumentException($"Cafe with ID {command.CafeId} not found");
            }

            var employee = _mapper.Map<EmployeeEntity>(command);
            employee.EmployeeId = GenerateEmployeeId();

            var result = await _employeeRepository.AddAsync(employee);
            return result.EmployeeId;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            return !string.IsNullOrEmpty(phoneNumber)
                && phoneNumber.Length == 8
                && (phoneNumber.StartsWith("8") || phoneNumber.StartsWith("9"))
                && phoneNumber.All(char.IsDigit);
        }

        private string GenerateEmployeeId()
        {
            // Generate ID in format UIXXXXXXX where X is alphanumeric
            return $"UI{Guid.NewGuid().ToString().Substring(0, 7).ToUpper()}";
        }
    }
}
