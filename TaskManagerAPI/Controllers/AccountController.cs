using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Data;
using TaskManagerAPI.Interfaces;
using TaskManagerAPI.Models;
using TaskManagerAPI.Models.DTOs.User;

namespace TaskManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<ApplicationUser> userManager,
            ITokenService tokenService,
            SignInManager<ApplicationUser> signInManager,
            IMapper mapper,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByNameAsync(model.Email);
            // Handle Invalid user
            if (user == null)
            {
                // Log warning
                _logger.LogWarning("Failed login attempt for email: {Email}", model.Email);
                return Unauthorized(new { message = "Invalid Email, username or password." });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (!result.Succeeded)
            {
                // Log warning
                _logger.LogWarning("Failed login attempt (wrong password) for email: {Email}", model.Email);
                return Unauthorized(new { message = "Invalid Email, username or password." });
            }

            // Store username is session
            HttpContext.Session.SetString("Username", user.UserName);

            // Generate token
            var token = _tokenService.CreateToken(user);
            if (token == null)
            {
                // Log critical error
                _logger.LogCritical("Token generation failed for user: {Username}", user.UserName);
                return Unauthorized(new { message = "Error generating token." });
            }

            // Log Information
            _logger.LogInformation("User {Username} logged in successfully. Email: {email}", user.UserName, user.Email);

            return Ok(new LoginResponseDto
            {
                Message = "Login successful.",
                Username = user.UserName,
                Token = token
            });
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check If user already exists
            if(await _userManager.FindByNameAsync(model.Email) != null)
            {
                // Log warning
                _logger.LogWarning("Registration attempt with existing email: {Email}", model.Email);
                return Unauthorized(new { message = "User with this email already exists." });
            }

            // Create new user
            var user = _mapper.Map<ApplicationUser>(model);
            user.UserName = model.Email;

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                // Log error
                _logger.LogError("User creation failed for email: {Email}. Errors: {Errors}", model.Email, result.Errors);
                return BadRequest(new { message = "Error creating user.", errors = result.Errors });
            }

            // Assign role to user
            await _userManager.AddToRoleAsync(user, model.Role);
            // Log Information
            _logger.LogInformation("User {Username} registered successfully with role {Role}. With Email: {Email}", user.UserName, model.Role, user.Email);
            return Ok(_mapper.Map<UserDto>(user));
        }
    }
}
