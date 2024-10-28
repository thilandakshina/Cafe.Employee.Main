using AutoMapper;
using Cafe.Business.Commands.Cafe;
using Cafe.Data.Repositories.Interfaces;
using MediatR;

namespace Cafe.Business.Handlers.Cafe
{
    public class UpdateCafeCommandHandler : IRequestHandler<UpdateCafeCommand, bool>
    {
        private readonly ICafeRepository _cafeRepository;
        private readonly IMapper _mapper;

        public UpdateCafeCommandHandler(ICafeRepository cafeRepository, IMapper mapper)
        {
            _cafeRepository = cafeRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateCafeCommand command, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(command.Name) || command.Name.Length < 6 || command.Name.Length > 10)
                throw new ArgumentException("Name must be between 6 and 10 characters");

            if (string.IsNullOrEmpty(command.Description) || command.Description.Length > 256)
                throw new ArgumentException("Description cannot exceed 256 characters");

            if (string.IsNullOrEmpty(command.Location))
                throw new ArgumentException("Location is required");

            var existingCafe = await _cafeRepository.GetByIdAsync(command.Id);
            if (existingCafe == null)
                throw new ArgumentException($"Cafe with ID {command.Id} not found");

            existingCafe.Name = command.Name;
            existingCafe.Description = command.Description;
            existingCafe.Location = command.Location;
            existingCafe.Logo = command.Logo;

            await _cafeRepository.UpdateAsync(existingCafe);
            return true;
        }
    }
}
