using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Security.Claims;
using TaskManagerAPI.Data;
using TaskManagerAPI.Interfaces;
using TaskManagerAPI.Models;
using TaskManagerAPI.Models.DTOs.Task;
using TaskManagerAPI.Repositories;

namespace TaskManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TasksController : Controller
    {
        private readonly ITaskRepository _taskRepository; private readonly IMapper _mapper;
        private readonly ILogger<TasksController> _logger;

        public TasksController(ITaskRepository taskRepository, IMapper mapper, ILogger<TasksController> logger)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == null)
            {
                // Log warning
                _logger.LogWarning("Unauthorized task creation attempt.");

                return Unauthorized(new { message = "Invalid User." });
            }

            var task = _mapper.Map<TaskItem>(model);
            task.ApplicationUserId = currentUserId;
            task.CreatedDate = DateTime.UtcNow;
            task.IsCompleted= false;

            // Save the task using the repository
            var createdTask = await _taskRepository.CreateTaskAsync(task);
            // Log information
            _logger.LogInformation("Task created with ID: {TaskId} by User ID: {UserId}", createdTask.Id, currentUserId);
            // Return the created task with a 201 status code
            return CreatedAtAction(nameof(GetTask), new { id = createdTask.Id }, _mapper.Map<TaskDto>(createdTask));
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetAllTasks()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == null)
            {
                // Log warning
                _logger.LogWarning("Unauthorized task creation attempt.");

                return Unauthorized(new { message = "Invalid User." });
            }

            // Get tasks using the repository
            var tasks = await _taskRepository.GetTasksForUserAsync(currentUserId);

            // Log information
            _logger.LogInformation("Retrieved {TaskCount} tasks for User ID: {UserId}", tasks.Count(), currentUserId);

            return Ok(_mapper.Map<IEnumerable<TaskDto>>(tasks));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskDto>> GetTask(int id)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == null)
            {
                _logger.LogWarning("Unauthorized task retrieval attempt.");
                return Unauthorized(new { message = "Invalid User." });
            }
            // Get the task using the repository
            var task = await _taskRepository.GetTaskAsync(currentUserId, id);

            if (task == null)
            {
                // Log Information
                _logger.LogInformation("Task with ID: {TaskId} not found for User ID: {UserId}.", id, currentUserId);
                return NotFound(new { message = "Task not found." });
            }

            // Log information
            _logger.LogInformation("Task with ID: {TaskId} retrieved by User ID: {UserId}.", id, currentUserId);

            return Ok(_mapper.Map<TaskDto>(task));
        }
    }
}
