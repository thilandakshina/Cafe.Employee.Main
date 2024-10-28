using Cafe.Business.DTOs;
using MediatR;

namespace Cafe.Business.Queries.Cafe
{
    public record GetCafeByIdQuery : IRequest<CafeDto>
    {
        public Guid Id { get; init; }
    }
}
