using NUnit.Framework;
using Moq;
using AutoMapper;
using Cafe.Business.Commands.Employee;
using Cafe.Business.Handlers.Employee;
using Cafe.Data.Entities;
using Cafe.Data.Repositories.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cafe.Business.Tests.Handlers
{
    [TestFixture]
    public class DeleteEmployeeCommandHandlerTests
    {
        private Mock<IEmployeeRepository> _mockEmployeeRepository;
        private Mock<ICafeEmployeeRepository> _mockCafeEmployeeRepository;
        private Mock<IMapper> _mockMapper;
        private DeleteEmployeeCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockEmployeeRepository = new Mock<IEmployeeRepository>();
            _mockCafeEmployeeRepository = new Mock<ICafeEmployeeRepository>();
            _mockMapper = new Mock<IMapper>();
            _handler = new DeleteEmployeeCommandHandler(
                _mockEmployeeRepository.Object,
                _mockCafeEmployeeRepository.Object,
                _mockMapper.Object);
        }

        [Test]
        public async Task Handle_WhenEmployeeExists_ShouldReturnTrue()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var command = new DeleteEmployeeCommand { Id = employeeId };

            var employee = new EmployeeEntity
            {
                Id = employeeId,
                IsActive = true
            };

            _mockEmployeeRepository.Setup(r => r.GetByIdAsync(employeeId))
                .ReturnsAsync(employee);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(employee.IsActive, Is.False);
        }

        [Test]
        public void Handle_WhenEmployeeNotFound_ShouldThrowException()
        {
            // Arrange
            var command = new DeleteEmployeeCommand { Id = Guid.NewGuid() };

            _mockEmployeeRepository.Setup(r => r.GetByIdAsync(command.Id))
                .ReturnsAsync((EmployeeEntity)null);

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _handler.Handle(command, CancellationToken.None));

            Assert.That(exception.Message, Is.EqualTo($"Employee with ID {command.Id} not found"));
        }
    }
}