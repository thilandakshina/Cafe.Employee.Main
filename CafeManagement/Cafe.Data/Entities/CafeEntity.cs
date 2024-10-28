namespace Cafe.Data.Entities
{
    public class CafeEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Logo { get; set; }
        public bool IsActive { get; set; } = true;
    }
}