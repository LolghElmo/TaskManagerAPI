using IdentityService.Application.DTOs.Auth;
using IdentityService.Application.Features.Login;
using IdentityService.Application.Features.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            // Create the RegisterCommand with the incoming request
            var command = new RegisterCommand
            {
                RegisterRequest = request
            };

            // Send the command to the mediator
            var result = await _mediator.Send(command);


            // Handle the result and return appropriate HTTP response
            if (!result.IsSuccess)
            {
                return BadRequest(new { Error = result.Error });
            }

            return Ok(result.Value);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            // Create the RegisterCommand with the incoming request
            var command = new LoginCommand
            {
                LoginRequest = request
            };

            // Send the command to the mediator
            var result = await _mediator.Send(command);

            // Handle the result and return appropriate HTTP response
            if (!result.IsSuccess)
            {
                return BadRequest(new { Error = result.Error });
            }

            return Ok(result.Value);
        }
    }
}
