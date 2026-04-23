using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TasksService.Application.DTOs;
using TasksService.Application.Features.Queries.GetAllTodos;
using TasksService.Application.Features.Queries.GetTodo;
using TasksService.Domain.Interfaces;
using TasksService.Domain.Models;

namespace TasksService.UnitTests.Application.Features.Queries
{
    public class GetTodoQueryHandlerTests
    {
        private readonly Mock<ITodoRepository> _todoRepositoryMock;
        private readonly Mock<ILogger<GetTodoQueryHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetTodoQueryHandler _handler;

        public GetTodoQueryHandlerTests()
        {
            _todoRepositoryMock = new Mock<ITodoRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<GetTodoQueryHandler>>();
            _handler = new GetTodoQueryHandler(
                                _todoRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
                );
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenGetTodoQuery()
        {
            // Arrange
            var query = new GetTodoQuery
            {
                UserId = "1234",
                Id = 1
            };
            var todo = new TodoItem
            {
                Id = query.Id,
                UserId = query.UserId,
                Title = "Test Todo",
                Description = "This is a test todo item."
            };
            _todoRepositoryMock.Setup(x => x.GetTodoByIdAsync(query.UserId, query.Id))
                               .ReturnsAsync(todo);
            var todoDto = new TodoDto
            {
                Id = query.Id,
                Title = todo.Title,
                Description = todo.Description
            };
            _mapperMock.Setup(x => x.Map<TodoDto>(todo))
                       .Returns(todoDto);
            // Act
            var result = await _handler.Handle(query, CancellationToken.None);
            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(todoDto);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenTodoNotFound()
        {
            // Arrange
            var query = new GetTodoQuery
            {
                UserId = "1234",
                Id = 1
            };
            _todoRepositoryMock.Setup(x => x.GetTodoByIdAsync(query.UserId, query.Id))
                               .ReturnsAsync((TodoItem)null);
            // Act
            var result = await _handler.Handle(query, CancellationToken.None);
            // Assert
            result.IsSuccess.Should().BeFalse();
        }
    }
}
