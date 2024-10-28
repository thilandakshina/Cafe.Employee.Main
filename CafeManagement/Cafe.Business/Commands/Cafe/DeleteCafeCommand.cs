using MediatR;

namespace Cafe.Business.Commands.Cafe
{
    public class DeleteCafeCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
