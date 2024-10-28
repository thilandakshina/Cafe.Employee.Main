using AutoMapper;
using Cafe.Business.Commands.Cafe;
using Cafe.Data.Entities;
using Cafe.Data.Repositories.Interfaces;
using MediatR;

namespace Cafe.Business.Handlers.Cafe
{
    public class CreateCafeCommandHandler : IRequestHandler<CreateCafeCommand, Guid>
    {
        private readonly ICafeRepository _cafeRepository;
        private readonly IMapper _mapper;

        public CreateCafeCommandHandler(ICafeRepository cafeRepository, IMapper mapper)
        {
            _cafeRepository = cafeRepository;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateCafeCommand command, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(command.Name) || command.Name.Length < 6 || command.Name.Length > 10)
                throw new ArgumentException("Name must be between 6 and 10 characters");

            if (string.IsNullOrEmpty(command.Description) || command.Description.Length > 256)
                throw new ArgumentException("Description cannot exceed 256 characters");

            if (string.IsNullOrEmpty(command.Location))
                throw new ArgumentException("Location is required");

            var cafe = _mapper.Map<CafeEntity>(command);
            var result = await _cafeRepository.AddAsync(cafe);
            return result.Id;
        }
    }
}
