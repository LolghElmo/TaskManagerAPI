using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagerAPI.Application.Features.Tasks;
using TaskManagerAPI.Application.Models.DTOs.Task;

namespace TaskManagerAPI.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TasksController : Controller
    {
        private readonly IMediator _mediator;
        public TasksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("CreateTask")]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto model) 
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Get the current user ID from the JWT token
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (currentUserId == null)
            {
                return Unauthorized(new { message = "Invalid User." });
            }

            // Create the command
            var command = new CreateTaskCommand
            {
                UserId = currentUserId,
                Name = model.Name,
                Description = model.Description,
                DueDate = model.DueDate
            };

            // Send the command to MediatR
            var createdTask = await _mediator.Send(command);

            // Return the created task with a 201 status code
            return CreatedAtAction(nameof(GetTaskById), new { id = createdTask.Id }, createdTask);
        }


        [HttpGet("GetAllTasks")]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetAllTasks()
        {
            // Get the current user ID from the JWT token
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == null)
            {
                return Unauthorized(new { message = "Invalid User." });
            }

            // Create the query
            var query = new GetAllTasksQuery
            {
                UserId = currentUserId
            };

            // Send the query to MediatR
            var tasks = await _mediator.Send(query);

            // Return the tasks
            return Ok(tasks);
        }
        [HttpGet("GetTaskById/{id}")]
        public async Task<ActionResult<TaskDto>> GetTaskById(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Get the current user ID from the JWT token
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Check if user is authenticated
            if (currentUserId == null)
            {
                return Unauthorized(new { message = "Invalid User." });
            }

            // Create the query
            var query = new GetTaskByIdQuery
            {
                UserId = currentUserId,
                TaskId = id
            };

            // Send the query to MediatR
            var task = await _mediator.Send(query);

            if (task == null)
            {
                return NotFound(new { message = "Task not found." });
            }

            // Return the task
            return Ok(task);
        }

    }
    /*    public class TasksController : Controller
        {
            private readonly ITaskRepository _taskRepository; private readonly IMapper _mapper;
            private readonly ILogger<TasksController> _logger;

            public TasksController(ITaskRepository taskRepository, IMapper mapper, ILogger<TasksController> logger)
            {
                _taskRepository = taskRepository;
                _mapper = mapper;
                _logger = logger;
            }
            #region USER CRUD OPERATIONS
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
                task.IsCompleted = false;

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
            [HttpPatch("{id}/finish")]
            public async Task<IActionResult> FinishTask(int id)
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (currentUserId == null)
                {
                    _logger.LogWarning("Unauthorized task finish attempt.");
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
                if (task.IsCompleted)
                {
                    // Log Information
                    _logger.LogInformation("Task with ID: {TaskId} is already completed by User ID: {UserId}.", id, currentUserId);
                    return BadRequest(new { message = "Task is already completed." });
                }
                // Mark the task as completed
                var updatedTask = await _taskRepository.FinishTaskAsync(task);
                // Log information
                _logger.LogInformation("Task with ID: {TaskId} marked as completed by User ID: {UserId}.", id, currentUserId);
                return Ok(_mapper.Map<TaskDto>(updatedTask));
            }
            #endregion

            #region ADMIN CRUD OPERATIONS
            [HttpDelete("{id}")]
            [Authorize(Roles = "Admin")]
            public async Task<IActionResult> DeleteTask(int id)
            {
                var task = await _taskRepository.GetTaskByIdAdminAsync(id);
                if (task == null)
                {
                    // Log Information
                    _logger.LogInformation("Admin attempted to delete non-existent Task ID: {TaskId}.", id);
                    return NotFound(new { message = "Task not found." });
                }
                var result = await _taskRepository.DeleteTaskAsync(task);
                if (!result)
                {
                    // Log Error
                    _logger.LogError("Failed to delete Task ID: {TaskId} by Admin.", id);
                    return StatusCode(500, new { message = "Failed to delete task." });
                }
                // Log information
                _logger.LogInformation("Task ID: {TaskId} deleted by Admin.", id);
                return NoContent();
            }
            [HttpPut("{id}")]
            [Authorize(Roles = "Admin")]
            public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskDto model)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var task = await _taskRepository.GetTaskByIdAdminAsync(id);
                if (task == null)
                {
                    // Log Information
                    _logger.LogInformation("Admin attempted to update non-existent Task ID: {TaskId}.", id);
                    return NotFound(new { message = "Task not found." });
                }
                // Map updated fields
                _mapper.Map(model, task);
                // Update the task using the repository
                var updatedTask = await _taskRepository.UpdateTaskAsync(task);
                if (updatedTask == null)
                {
                    // Log Error
                    _logger.LogError("Failed to update Task ID: {TaskId} by Admin.", id);
                    return StatusCode(500, new { message = "Failed to update task." });
                }
                // Log information
                _logger.LogInformation("Task ID: {TaskId} updated by Admin.", id);
                return Ok(_mapper.Map<TaskDto>(updatedTask));
            }
            #endregion
        }
    */

}
