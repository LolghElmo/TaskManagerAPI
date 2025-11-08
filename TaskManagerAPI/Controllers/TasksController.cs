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
    [Authorize]
    public class TasksController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public TasksController(DataContext dataContext, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _dataContext = dataContext;
            _userManager = userManager;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto model)
        {
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == null)
                return Unauthorized();

            var task = _mapper.Map<TaskItem>(model);
            task.ApplicationUserId = currentUserId;
            task.CreatedDate = DateTime.UtcNow;
            task.IsCompleted= false;

            await _dataContext.Items.AddAsync(task);
            await _dataContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, _mapper.Map<TaskDto>(task));
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetMyTasks()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == null)
                return Unauthorized();

            var tasks = await _dataContext.Items
                        .Where(t => t.ApplicationUserId == currentUserId)
                        .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<TaskDto>>(tasks));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskDto>> GetTask(int id)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var task = await _dataContext.Items
                        .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null) return NotFound();
            if (task.ApplicationUserId != currentUserId) return Forbid();

            return Ok(_mapper.Map<TaskDto>(task));
        }
    }
}
