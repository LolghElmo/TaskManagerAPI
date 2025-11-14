using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TaskManagerAPI.Controllers;
using TaskManagerAPI.Interfaces;
using TaskManagerAPI.Models;
using TaskManagerAPI.Models.DTOs.Task;
using TaskManagerAPI.Utility;
using Xunit;

namespace TaskManagerAPI.UnitTest
{
    public class TasksControllerTests
    {
        private static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfiles>());
            return config.CreateMapper();
        }

        private static ClaimsPrincipal CreateUser(string userId)
        {
            return new ClaimsPrincipal(new ClaimsIdentity(
                new[] { new Claim(ClaimTypes.NameIdentifier, userId) },
                "TestAuthentication"));
        }

        [Fact]
        public async Task CreateTask_ReturnsCreatedAtAction_WhenModelIsValid()
        {
            var userId = "user-123";
            var mockRepo = new Mock<ITaskRepository>(MockBehavior.Strict);
            var mapper = CreateMapper();
            var logger = Mock.Of<ILogger<TasksController>>();
            var newTask = new TaskItem { Id = 1, Name = "Test Task", Description = "Description", DueDate = DateTime.UtcNow.AddDays(1), ApplicationUserId = userId };
            mockRepo.Setup(r => r.CreateTaskAsync(It.IsAny<TaskItem>())).ReturnsAsync(newTask);

            var controller = new TasksController(mockRepo.Object, mapper, logger)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = CreateUser(userId) }
                }
            };

            var dto = new CreateTaskDto { Name = newTask.Name, Description = newTask.Description, DueDate = newTask.DueDate };
            var result = await controller.CreateTask(dto);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(201, created.StatusCode);
            var returnedTask = Assert.IsType<TaskDto>(created.Value);
            Assert.Equal(newTask.Name, returnedTask.Name);
            mockRepo.Verify(r => r.CreateTaskAsync(It.IsAny<TaskItem>()), Times.Once);
        }

        [Fact]
        public async Task CreateTask_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            var mockRepo = new Mock<ITaskRepository>();
            var controller = new TasksController(mockRepo.Object, CreateMapper(), Mock.Of<ILogger<TasksController>>());
            controller.ModelState.AddModelError("Name", "Name is required");

            var result = await controller.CreateTask(new CreateTaskDto());
            Assert.IsType<BadRequestObjectResult>(result);
            mockRepo.Verify(r => r.CreateTaskAsync(It.IsAny<TaskItem>()), Times.Never);
        }

        [Fact]
        public async Task CreateTask_ReturnsUnauthorized_WhenUserIdMissing()
        {
            var mockRepo = new Mock<ITaskRepository>();
            var controller = new TasksController(mockRepo.Object, CreateMapper(), Mock.Of<ILogger<TasksController>>())
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };

            var dto = new CreateTaskDto { Name = "Task", DueDate = DateTime.UtcNow.AddDays(1) };
            var result = await controller.CreateTask(dto);

            Assert.IsType<UnauthorizedObjectResult>(result);
            mockRepo.Verify(r => r.CreateTaskAsync(It.IsAny<TaskItem>()), Times.Never);
        }

        [Fact]
        public async Task GetAllTasks_ReturnsOkWithMappedTasks()
        {
            var userId = "user-123";
            var tasks = new List<TaskItem>
            {
                new TaskItem { Id = 1, Name = "T1", Description = "D1", DueDate = DateTime.UtcNow.AddDays(1), ApplicationUserId = userId },
                new TaskItem { Id = 2, Name = "T2", Description = "D2", DueDate = DateTime.UtcNow.AddDays(2), ApplicationUserId = userId }
            };
            var mockRepo = new Mock<ITaskRepository>(MockBehavior.Strict);
            mockRepo.Setup(r => r.GetTasksForUserAsync(userId)).ReturnsAsync(tasks);

            var controller = new TasksController(mockRepo.Object, CreateMapper(), Mock.Of<ILogger<TasksController>>())
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = CreateUser(userId) }
                }
            };

            var result = await controller.GetAllTasks();
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var returned = Assert.IsAssignableFrom<IEnumerable<TaskDto>>(ok.Value);
            Assert.Equal(2, returned.Count());
            mockRepo.Verify(r => r.GetTasksForUserAsync(userId), Times.Once);
        }

        [Fact]
        public async Task GetTask_ReturnsNotFound_WhenTaskMissing()
        {
            var userId = "user-123";
            var mockRepo = new Mock<ITaskRepository>(MockBehavior.Strict);
            mockRepo.Setup(r => r.GetTaskAsync(userId, 42)).ReturnsAsync((TaskItem?)null);

            var controller = new TasksController(mockRepo.Object, CreateMapper(), Mock.Of<ILogger<TasksController>>())
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = CreateUser(userId) } }
            };

            var result = await controller.GetTask(42);
            Assert.IsType<NotFoundObjectResult>(result.Result);
            mockRepo.Verify(r => r.GetTaskAsync(userId, 42), Times.Once);
        }

        [Fact]
        public async Task GetTask_ReturnsOkWithTask_WhenTaskExists()
        {
            var userId = "user-123";
            var task = new TaskItem { Id = 3, Name = "Existing", Description = "Desc", DueDate = DateTime.UtcNow.AddDays(1), ApplicationUserId = userId };
            var mockRepo = new Mock<ITaskRepository>(MockBehavior.Strict);
            mockRepo.Setup(r => r.GetTaskAsync(userId, 3)).ReturnsAsync(task);

            var controller = new TasksController(mockRepo.Object, CreateMapper(), Mock.Of<ILogger<TasksController>>())
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = CreateUser(userId) } }
            };

            var result = await controller.GetTask(3);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var dto = Assert.IsType<TaskDto>(okResult.Value);
            Assert.Equal(task.Name, dto.Name);
            Assert.Equal(task.Description, dto.Description);
            mockRepo.Verify(r => r.GetTaskAsync(userId, 3), Times.Once);
        }
    }
}
