namespace Cafe.Business.DTOs
{
    public class CafeEmployeeDto
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; } //Needs to change
        public Guid CafeId { get; set; }
        public string CafeName { get; set; } //Needs to change
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
