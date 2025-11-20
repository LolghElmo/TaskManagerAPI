using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using TaskManagerAPI.Application.Features.Account;
using TaskManagerAPI.Core.Interfaces;
using TaskManagerAPI.Core.Models;

namespace TaskManagerAPI.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AccountController : Controller
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            // Validate the incoming command
            try
            {
                // Send the command to the mediator
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here for brevity)
                return Unauthorized(new { message = ex.Message });
            }
            
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            // Validate the incoming command
            try
            {
                // Send the command to the mediator
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here for brevity)
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
