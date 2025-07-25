using API.DTO;
using API.helper.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.DependencyResolver;

namespace API.Controllers
{
    [ApiController]
    [Route("API/login")]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtTokenService _tokenService;

        public AuthController(SignInManager<IdentityUser> singinManager,
            UserManager<IdentityUser> userManager,
            JwtTokenService tokenService)
        {
            _signInManager = singinManager;
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginModel)
        {
            var user = await _userManager.FindByNameAsync(loginModel.Username);
            if (user == null)  return Unauthorized();

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);
            if (!result.Succeeded) return Unauthorized();

            var token = _tokenService.GenerateToken(user);
            return Ok(token);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO register)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //if (await _userManager.FindByNameAsync(register.userName) != null)
            //    return BadRequest("User already exists");

            //if (await _userManager.FindByEmailAsync(register.email) != null)
            //    return BadRequest("User already exists");

            var user = new IdentityUser
            {
                UserName = register.userName,
                Email = register.email
            };

            var result = await _userManager.CreateAsync(user, register.password);
            if (!result.Succeeded) return BadRequest(result.Errors.ToString());

            result = await _userManager.AddToRoleAsync(user, "User");
            if (!result.Succeeded) return BadRequest(result.Errors.ToString());

            return Ok("User created successfully!");

        }
    }
}
