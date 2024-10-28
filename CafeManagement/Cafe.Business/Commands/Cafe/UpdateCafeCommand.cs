using MediatR;

namespace Cafe.Business.Commands.Cafe
{
    public class UpdateCafeCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Logo { get; set; }
    }
}
