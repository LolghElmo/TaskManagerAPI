using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TasksService.Application.DTOs;
using TasksService.Application.Features.Commands.CreateTodo;
using TasksService.Domain.Interfaces;
using TasksService.Domain.Models;

namespace TasksService.UnitTests.Application.Features.Commands
{
    public class CreateTodoCommandHandlerTests
    {
        private readonly Mock<ITodoRepository> _todoRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<CreateTodoCommandHandler>> _loggerMock;
        private readonly CreateTodoCommandHandler _handler;

        public CreateTodoCommandHandlerTests()
        {
            _todoRepositoryMock = new Mock<ITodoRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<CreateTodoCommandHandler>>();
            _handler = new CreateTodoCommandHandler(
                _todoRepositoryMock.Object,
                _loggerMock.Object,
                _mapperMock.Object
                );
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenCreateTodoCommand()
        {
            // Arrange
            var command = new CreateTodoCommand
            {
                UserId = "1234",
                TodoRequest = new CreateTodoRequest
                {
                    Title = "Test Todo",
                    Description = "This is a test todo item."
                }
            };

            var mappedTodo = new TodoItem
            {
                UserId = command.UserId,
                Title = command.TodoRequest.Title,
                Description = command.TodoRequest.Description
            };
            _mapperMock.Setup(x => x.Map<TodoItem>(command.TodoRequest))
                       .Returns(mappedTodo);

            var todoDto = new TodoDto
            {
                Id = 1,
                Title = command.TodoRequest.Title,
                Description = command.TodoRequest.Description,
            };
            _mapperMock.Setup(x => x.Map<TodoDto>(It.IsAny<TodoItem>()))
                       .Returns(todoDto);
            _todoRepositoryMock.Setup(x => x.CreateTodoAsync(It.IsAny<TodoItem>()))
                   .ReturnsAsync(mappedTodo);
            // Act 
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]

        public async Task Handle_ShouldReturnFailure_WhenRepositoryReturnsNull()
        {
            // Arrange
            var command = new CreateTodoCommand
            {
                UserId = "1234",
                TodoRequest = new CreateTodoRequest 
                {
                    Title = "X", 
                    Description = "Y"
                }
            };
            var todoDto = new TodoDto
            {
                Id = 1,
                Title = command.TodoRequest.Title,
                Description = command.TodoRequest.Description,
            };
            var mappedTodo = new TodoItem
            {
                UserId = command.UserId,
                Title = command.TodoRequest.Title,
                Description = command.TodoRequest.Description
            };

            _mapperMock.Setup(x => x.Map<TodoItem>(command.TodoRequest))
                .Returns(mappedTodo);

            _mapperMock.Setup(x => x.Map<TodoDto>(It.IsAny<TodoItem>()))
                .Returns(todoDto);

            _todoRepositoryMock.Setup(x => x.CreateTodoAsync(It.IsAny<TodoItem>()))
                               .ReturnsAsync((TodoItem?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
        }

    }
}
