namespace Cafe.Data.Entities
{
    public class EmployeeEntity
    {
        public Guid Id { get; set; }
        public string EmployeeId { get; set; } // UIXXXXXXX format
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public GenderType Gender { get; set; } = GenderType.Male;
        public bool IsActive { get; set; } = true;

    }

    public enum GenderType
    {
        Male,
        Female,
        Other
    }
}
