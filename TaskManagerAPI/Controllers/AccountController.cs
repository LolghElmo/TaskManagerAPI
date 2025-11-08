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
    public class AccountController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;

        public AccountController(DataContext context,
            UserManager<ApplicationUser> userManager,
            ITokenService tokenService,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _tokenService = tokenService;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
                return Unauthorized(new { message = "Invalid Email, username or password." });
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded)
                return Unauthorized(new { message = "Invalid Email, username or password." });

            // Store username is session
            HttpContext.Session.SetString("Username", user.UserName);

            // Generate token
            var token = _tokenService.CreateToken(user);
            if (token == null)
                return Unauthorized(new { message = "Error generating token." });

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
                return BadRequest(new { message = "User with this email already exists." });

            // Create new user
            var user = _mapper.Map<ApplicationUser>(model);
            user.UserName = model.Email;

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return BadRequest(new { message = "Error creating user.", errors = result.Errors });

            // Assign role to user
            await _userManager.AddToRoleAsync(user, model.Role);

            return Ok(_mapper.Map<UserDto>(user));
        }
    }
}
