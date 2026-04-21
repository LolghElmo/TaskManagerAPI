using System;
using System.Collections.Generic;
using System.Text;
using TasksService.Application.DTOs;
using TasksService.Application.Features.Commands.CreateTodo;
using TasksService.Application.Features.Queries.GetAllTodos;

namespace TasksService.IntegrationTests
{
    public class AuthTest : BaseIntegrationTest
    {
        public AuthTest(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }
        [Fact]
        public async Task CreateTodo_AddsTodoForUser()
        {
            // Arrange
            var command = new CreateTodoCommand 
            { 
                UserId = "user-123",
                TodoRequest = new CreateTodoRequest 
                { 
                    Title = "Test Task", 
                    Description = "This is a test task." 
                }
            };

            // Act
            var result = await Sender.Send(command);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task GetAllTodos_ReturnsOnlyThisUsersTodos()
        {
            // Arrange 
            await Sender.Send(
                new CreateTodoCommand
                {
                    UserId = "user-a",
                    TodoRequest = new CreateTodoRequest
                    {
                        Title = "Test A Task",
                        Description = "This is a test task."
                    }
                });

            await Sender.Send(
                new CreateTodoCommand
                {
                    UserId = "user-b",
                    TodoRequest = new CreateTodoRequest
                    {
                        Title = "Test B Task",
                        Description = "This is b test task."
                    }
                });

            // Act
            var result = await Sender.Send(new GetAllTodosQuery { UserId = "user-A", Page = 1, PageSize = 10 });

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Single(result.Value.Items);
            Assert.Equal("Test A Task", result.Value.Items.First().Title);
        }
    }
}
