using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TasksService.Application.DTOs;
using TasksService.Application.Features.Commands.CreateTodo;
using TasksService.Application.Features.Queries.GetAllTodos;
using TasksService.Domain.Interfaces;
using TasksService.Domain.Models;

namespace TasksService.UnitTests.Application.Features.Queries
{
    public class GetAllTodosQueryHandlerTests
    {
        private readonly Mock<ITodoRepository> _todoRepositoryMock;
        private readonly Mock<ILogger<GetAllTodosQueryHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllTodosQueryHandler _handler;

        public GetAllTodosQueryHandlerTests()
        {
            _todoRepositoryMock = new Mock<ITodoRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<GetAllTodosQueryHandler>>();
            _handler = new GetAllTodosQueryHandler(
                _mapperMock.Object,
                _todoRepositoryMock.Object,
                _loggerMock.Object
                );
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenGetAllTodosQuery()
        {
            // Arrange
            var query = new GetAllTodosQuery
            {
                UserId = "1234",
                Page = 1,
                PageSize = 10
            };
            var todos = new List<TodoItem>
            {
                new TodoItem {
                    Id = 1,
                    UserId = query.UserId,
                    Title = "Test Todo 1",
                    Description = "This is a test todo item."
                },
                new TodoItem { Id = 2,
                    UserId = query.UserId,
                    Title = "Test Todo 2",
                    Description = "This is another test todo item."
                }
            };
            _todoRepositoryMock.Setup(x => x.GetAllTodosAsync(query.UserId, query.Page, query.PageSize))
                               .ReturnsAsync((todos, todos.Count));
            var todoDtos = new List<TodoDto>
            {
                new TodoDto { Id = 1,
                    Title = "Test Todo 1",
                    Description = "This is a test todo item."
                },
                new TodoDto { Id = 2,
                    Title = "Test Todo 2",
                    Description = "This is another test todo item." 
                }
            };
            _mapperMock.Setup(x => x.Map<IEnumerable<TodoDto>>(todos))
                       .Returns(todoDtos);
            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Items.Should().HaveCount(2);
            result.Value.TotalCount.Should().Be(2);
            result.Value.Page.Should().Be(query.Page);
            result.Value.PageSize.Should().Be(query.PageSize);
        }
    }
}
