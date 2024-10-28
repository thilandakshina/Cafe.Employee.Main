using Cafe.Business.Commands.Cafe;
using Cafe.Data.Repositories.Interfaces;
using MediatR;

namespace Cafe.Business.Handlers.Cafe
{
    public class DeleteCafeCommandHandler : IRequestHandler<DeleteCafeCommand, bool>
    {
        private readonly ICafeRepository _cafeRepository;
        private readonly ICafeEmployeeRepository _cafeEmployeeRepository;

        public DeleteCafeCommandHandler(ICafeRepository cafeRepository, ICafeEmployeeRepository cafeEmployeeRepository)
        {
            _cafeRepository = cafeRepository;
            _cafeEmployeeRepository = cafeEmployeeRepository;
        }

        public async Task<bool> Handle(DeleteCafeCommand command, CancellationToken cancellationToken)
        {
            var cafe = await _cafeRepository.GetByIdAsync(command.Id);
            if (cafe == null)
                throw new ArgumentException($"Cafe with ID {command.Id} not found");

            await _cafeEmployeeRepository.UnassignListofEmployeesFromCafeAsync(command.Id);
            
            cafe.IsActive = false;
            await _cafeRepository.UpdateAsync(cafe);
            return true;
        }
    }
}
