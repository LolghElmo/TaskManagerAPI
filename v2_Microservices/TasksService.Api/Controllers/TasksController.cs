using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TasksService.Application.DTOs;
using TasksService.Application.Features.Commands.CreateTodo;
using TasksService.Application.Features.Commands.DeleteTodo;
using TasksService.Application.Features.Commands.UpdateTodo;
using TasksService.Application.Features.Queries.GetAllTodos;
using TasksService.Application.Features.Queries.GetTodo;

namespace TasksService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TasksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllTodosQuery { UserId = GetUserId() };
            var result = await _mediator.Send(query);
            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetTodoQuery { UserId = GetUserId(), Id = id };
            var result = await _mediator.Send(query);

            if (!result.IsSuccess) return NotFound(result.Error);
            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTodoRequest request)
        {
            var command = new CreateTodoCommand
            {
                UserId = GetUserId(),
                TodoRequest = request
            };

            var result = await _mediator.Send(command);

            if (!result.IsSuccess) return BadRequest(result.Error);

            return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTodoRequest request)
        {
            var command = new UpdateTodoCommand
            {
                UserId = GetUserId(),
                Id = id,
                TodoRequest = request
            };

            var result = await _mediator.Send(command);

            if (!result.IsSuccess) return NotFound(result.Error);
            return Ok(result.Value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteTodoCommand
            {
                UserId = GetUserId(),
                Id = id
            };

            var result = await _mediator.Send(command);

            if (!result.IsSuccess) return NotFound(result.Error);
            return NoContent();
        }
    }
}
