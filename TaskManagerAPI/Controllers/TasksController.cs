using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskManagerAPI.Data;
using TaskManagerAPI.Models;
using TaskManagerAPI.Models.ViewModels;

namespace TaskManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public TasksController(DataContext dataContext, UserManager<ApplicationUser> userManager)
        {
            this._dataContext = dataContext;
            this._userManager = userManager;
        }
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == null)
            {
                return Unauthorized();
            }

            var task = new TaskItem
            {
                Name = model.Name,
                Description = model.Description,
                DueDate = model.DueDate,
                IsCompleted = false,
                CreatedDate = DateTime.UtcNow,
                ApplicationUserId = currentUserId 
            };

            await _dataContext.Items.AddAsync(task);
            await _dataContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskViewModel>>> GetMyTasks()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == null)
            {
                return Unauthorized();
            }

            var tasks = await _dataContext.Items
                .Where(t => t.ApplicationUserId == currentUserId)
                .Select(t => new TaskViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    Description = t.Description,
                    IsCompleted = t.IsCompleted,
                    CreatedDate = t.CreatedDate,
                    DueDate = t.DueDate
                })
                .ToListAsync();

            return Ok(tasks);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskViewModel>> GetTask(int id)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var task = await _dataContext.Items
                .Where(t => t.Id == id) 
                .Select(t => new TaskViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    Description = t.Description,
                    IsCompleted = t.IsCompleted,
                    CreatedDate = t.CreatedDate,
                    DueDate = t.DueDate,
                    ApplicationUserId = t.ApplicationUserId
                })
                .FirstOrDefaultAsync();

            if (task == null)
            {
                return NotFound();
            }

            if (task.ApplicationUserId != currentUserId)
            {
                return Forbid();
            }

            return Ok(task);
        }

        public class TaskViewModel : Models.ViewModels.TaskViewModel
        {
            public string ApplicationUserId { get; set; }
        }
    }
}
