using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;
using TaskManagerAPI.Data;
using TaskManagerAPI.Models;
using TaskManagerAPI.Models.DTOs.Task;

namespace TaskManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly ILogger<TasksController> _logger;

        public TasksController(DataContext dataContext, IMapper mapper, ILogger<TasksController> logger)
        {
            _dataContext = dataContext;
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

            await _dataContext.Items.AddAsync(task);
            await _dataContext.SaveChangesAsync();

            // Log information
            _logger.LogInformation("Task created with ID: {TaskId} by User ID: {UserId}", task.Id, currentUserId);

            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, _mapper.Map<TaskDto>(task));
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetMyTasks()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == null)
            {
                // Log warning
                _logger.LogWarning("Unauthorized task creation attempt.");

                return Unauthorized(new { message = "Invalid User." });
            }

            var tasks = await _dataContext.Items
                        .Where(t => t.ApplicationUserId == currentUserId)
                        .ToListAsync();

            // Log information
            _logger.LogInformation("Retrieved {TaskCount} tasks for User ID: {UserId}", tasks.Count, currentUserId);

            return Ok(_mapper.Map<IEnumerable<TaskDto>>(tasks));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskDto>> GetTask(int id)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var task = await _dataContext.Items
                        .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
            {
                // Log information
                _logger.LogInformation("Task with ID: {TaskId} not found.", id);

                return NotFound(new { message = "Task not found."});
            }
            if (task.ApplicationUserId != currentUserId)
            {
                // Log error
                _logger.LogError("User ID: {UserId} attempted to access Task ID: {TaskId} without permission.", currentUserId, id);

                return Forbid();
            }
            // Log information
            _logger.LogInformation("Task with ID: {TaskId} retrieved by User ID: {UserId}.", id, currentUserId);

            return Ok(_mapper.Map<TaskDto>(task));
        }
    }
}
