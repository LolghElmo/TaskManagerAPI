using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Data;
using TaskManagerAPI.Interfaces;
using TaskManagerAPI.Models;
using TaskManagerAPI.Models.ViewModels;

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

        public AccountController(DataContext context,
            UserManager<ApplicationUser> userManager,
            ITokenService tokenService,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _tokenService = tokenService;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginViewModels model)
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

            return Ok(new
            {
                message = "Login successful.",
                username = user.UserName,
                token = token
            });
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check If user already exists
            if(await _userManager.FindByNameAsync(model.Email) != null)
                return BadRequest(new { message = "User with this email already exists." });

            // Create new user
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return BadRequest(new { message = "Error creating user.", errors = result.Errors });
            await _userManager.AddToRoleAsync(user, model.Role);
            return Ok(new { 
                message = "User registered successfully.", 
                username = user.UserName,
            });
        }
    }
}
