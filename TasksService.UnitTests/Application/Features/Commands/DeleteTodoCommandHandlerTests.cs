using Microsoft.Extensions.Logging;
using Moq;
using FluentAssertions;
using TasksService.Application.Features.Commands.DeleteTodo;
using TasksService.Domain.Interfaces;
using TasksService.Domain.Models;

namespace TasksService.UnitTests.Application.Features.Commands
{
    public class DeleteTodoCommandHandlerTests
    {
        private readonly Mock<ITodoRepository> _todoRepositoryMock;
        private readonly Mock<ILogger<DeleteTodoCommandHandler>> _loggerMock;
        private readonly DeleteTodoCommandHandler _handler;

        public DeleteTodoCommandHandlerTests()
        {
            _todoRepositoryMock = new Mock<ITodoRepository>();
            _loggerMock = new Mock<ILogger<DeleteTodoCommandHandler>>();
            _handler = new DeleteTodoCommandHandler(
                _todoRepositoryMock.Object,
                _loggerMock.Object
                );
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenTodoIsDeleted()
        {
            // Arrange
            var command = new DeleteTodoCommand
            {
                UserId = "1234",
                Id = 1
            };
            var existingTodo = new TodoItem
            {
                Id = command.Id,
                UserId = command.UserId,
                Title = "Test Todo",
                Description = "This is a test todo item."
            };

            _todoRepositoryMock.Setup(x => x.GetTodoByIdAsync(command.UserId, command.Id))
                               .ReturnsAsync(existingTodo);
            // Act
            var result = await _handler.Handle(command, CancellationToken.None);
            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeTrue();
            _todoRepositoryMock.Verify(x => x.DeleteTodoAsync(existingTodo), Times.Once);
        }
        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenTodoNotFound()
        {
            // Arrange
            var command = new DeleteTodoCommand
            {
                UserId = "1234",
                Id = 1
            };
            _todoRepositoryMock.Setup(x => x.GetTodoByIdAsync(command.UserId, command.Id))
                               .ReturnsAsync((TodoItem)null);
            // Act
            var result = await _handler.Handle(command, CancellationToken.None);
            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Todo item not found.");
            _todoRepositoryMock.Verify(x => x.DeleteTodoAsync(It.IsAny<TodoItem>()), Times.Never);
        }
    }
}
