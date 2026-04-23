using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using TasksService.Application.DTOs;
using TasksService.Application.Features.Commands.DeleteTodo;
using TasksService.Application.Features.Commands.UpdateTodo;
using TasksService.Domain.Interfaces;
using TasksService.Domain.Models;

namespace TasksService.UnitTests.Application.Features.Commands
{
    public class UpdateTodoCommandHandlerTests
    {
        private readonly Mock<ITodoRepository> _todoRepositoryMock;
        private readonly Mock<ILogger<UpdateTodoCommandHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateTodoCommandHandler _handler;


        public UpdateTodoCommandHandlerTests()
        {
            _todoRepositoryMock = new Mock<ITodoRepository>();
            _loggerMock = new Mock<ILogger<UpdateTodoCommandHandler>>();
            _mapperMock = new Mock<IMapper>();
            _handler = new UpdateTodoCommandHandler(
                _todoRepositoryMock.Object,
                _loggerMock.Object,
                _mapperMock.Object
                );
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenTodoIsUpdated()
        {
            // Arrange
            var command = new UpdateTodoCommand
            {
                UserId = "1234",
                Id = 1,
                TodoRequest = new UpdateTodoRequest
                {
                    Title = "Updated Test Todo",
                    Description = "This is an updated test todo item."
                }
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
            _mapperMock.Verify(x => x.Map(command.TodoRequest, existingTodo), Times.Once);
            _todoRepositoryMock.Verify(x => x.UpdateTodoAsync(existingTodo), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenTodoNotFound()
        {
            // Arrange
            var command = new UpdateTodoCommand
            {
                UserId = "1234",
                Id = 1,
                TodoRequest = new UpdateTodoRequest
                {
                    Title = "Updated Test Todo",
                    Description = "This is an updated test todo item."
                }
            };
            _todoRepositoryMock.Setup(x => x.GetTodoByIdAsync(command.UserId, command.Id))
                               .ReturnsAsync((TodoItem)null);
            // Act
            var result = await _handler.Handle(command, CancellationToken.None);
            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Todo item not found.");
        }
    }
}
