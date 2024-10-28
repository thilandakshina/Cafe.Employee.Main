using AutoMapper;
using Cafe.Business.DTOs;
using Cafe.Business.Queries.Cafe;
using Cafe.Data.Repositories.Interfaces;
using MediatR;

namespace Cafe.Business.Handlers.Cafe
{
    public class GetCafesQueryHandler : IRequestHandler<GetCafesQuery, IEnumerable<CafeDto>>
    {
        private readonly ICafeRepository _cafeRepository;
        private readonly IMapper _mapper;

        public GetCafesQueryHandler(ICafeRepository cafeRepository, IMapper mapper)
        {
            _cafeRepository = cafeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CafeDto>> Handle(GetCafesQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var cafes = string.IsNullOrEmpty(query.Location)
                    ? await _cafeRepository.GetAllAsync()
                    : await _cafeRepository.GetByLocationAsync(query.Location);

                var cafeDtos = _mapper.Map<IEnumerable<CafeDto>>(cafes).ToList();

                foreach (var dto in cafeDtos)
                {
                    dto.EmployeeCount = await _cafeRepository.GetEmployeeCountAsync(dto.Id);
                }

                return cafeDtos.OrderByDescending(x => x.EmployeeCount);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving cafes", ex);
            }
        }
    }
}