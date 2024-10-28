using static Cafe.Business.Common;

namespace Cafe.Business.DTOs
{
    public class EmployeeDto
    {
        public Guid Id { get; set; }
        public string EmployeeId { get; set; } // UIXXXXXXX format
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public GenderType Gender { get; set; } = GenderType.Male;
        public int DaysWorked { get; set; }
        public string CafeName { get; set; } = string.Empty;
        public Guid? CafeId { get; set; }
    }
}
