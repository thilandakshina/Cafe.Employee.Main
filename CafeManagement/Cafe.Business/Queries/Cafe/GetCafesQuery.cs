using Cafe.Business.DTOs;
using MediatR;

namespace Cafe.Business.Queries.Cafe
{
    public record GetCafesQuery : IRequest<IEnumerable<CafeDto>>
    {
        public string? Location { get; init; }
    }
}
