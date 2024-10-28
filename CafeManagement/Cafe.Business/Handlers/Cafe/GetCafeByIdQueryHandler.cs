using AutoMapper;
using Cafe.Business.DTOs;
using Cafe.Business.Queries.Cafe;
using Cafe.Data.Repositories.Interfaces;
using MediatR;

namespace Cafe.Business.Handlers.Cafe
{
    public class GetCafeByIdQueryHandler : IRequestHandler<GetCafeByIdQuery, CafeDto>
    {
        private readonly ICafeRepository _cafeRepository;
        private readonly IMapper _mapper;

        public GetCafeByIdQueryHandler(ICafeRepository cafeRepository, IMapper mapper)
        {
            _cafeRepository = cafeRepository;
            _mapper = mapper;
        }

        public async Task<CafeDto> Handle(GetCafeByIdQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var cafe = await _cafeRepository.GetCafeByIdAsync(query.Id);
                var cafeDtos = _mapper.Map<CafeDto>(cafe);
                cafeDtos.EmployeeCount = await _cafeRepository.GetEmployeeCountAsync(cafeDtos.Id);

                return cafeDtos;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving cafe", ex);
            }
        }
    }
}