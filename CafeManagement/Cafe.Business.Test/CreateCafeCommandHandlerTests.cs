using Moq;
using AutoMapper;
using Cafe.Business.Commands.Cafe;
using Cafe.Business.Handlers.Cafe;
using Cafe.Data.Entities;
using Cafe.Data.Repositories.Interfaces;

namespace Cafe.Business.Tests.Handlers
{
    [TestFixture]
    public class CreateCafeCommandHandlerTests
    {
        private Mock<ICafeRepository> _mockRepository;
        private Mock<IMapper> _mockMapper;
        private CreateCafeCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<ICafeRepository>();
            _mockMapper = new Mock<IMapper>();
            _handler = new CreateCafeCommandHandler(_mockRepository.Object, _mockMapper.Object);
        }

        [Test]
        public async Task Handle_ValidCommand_ReturnsGuid()
        {
            // Arrange
            var command = new CreateCafeCommand
            {
                Name = "Cafe Test",
                Description = "Test Description",
                Location = "Test Location",
                Logo = "test-logo.png"
            };

            var cafeEntity = new CafeEntity
            {
                Id = Guid.NewGuid(),
                Name = command.Name,
                Description = command.Description,
                Location = command.Location,
                Logo = command.Logo
            };

            _mockMapper.Setup(m => m.Map<CafeEntity>(command))
                .Returns(cafeEntity);

            _mockRepository.Setup(r => r.AddAsync(It.IsAny<CafeEntity>()))
                .ReturnsAsync(cafeEntity);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.EqualTo(cafeEntity.Id));
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<CafeEntity>()), Times.Once);
        }

        [Test]
        public void Handle_InvalidName_ThrowsArgumentException()
        {
            // Arrange
            var command = new CreateCafeCommand
            {
                Name = "Test", // Less than 6 characters
                Description = "Test Description",
                Location = "Test Location"
            };

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _handler.Handle(command, CancellationToken.None));

            Assert.That(exception.Message, Is.EqualTo("Name must be between 6 and 10 characters"));
        }

        [Test]
        public void Handle_EmptyLocation_ThrowsArgumentException()
        {
            // Arrange
            var command = new CreateCafeCommand
            {
                Name = "Valid Name",
                Description = "Test Description",
                Location = string.Empty
            };

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _handler.Handle(command, CancellationToken.None));

            Assert.That(exception.Message, Is.EqualTo("Location is required"));
        }

        [Test]
        public void Handle_LongDescription_ThrowsArgumentException()
        {
            // Arrange
            var command = new CreateCafeCommand
            {
                Name = "Valid Name",
                Description = new string('x', 257), // 257 characters
                Location = "Test Location"
            };

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _handler.Handle(command, CancellationToken.None));

            Assert.That(exception.Message, Is.EqualTo("Description cannot exceed 256 characters"));
        }
    }
}