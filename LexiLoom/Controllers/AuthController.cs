using LexiLoom.DTO;
using LexiLoom.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LexiLoom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginModel authModel)
        {
            var result = await _authService.AuthenticateUser(authModel);
            if (result == null)
            {
                return Unauthorized();
            }

            var jwt = _authService.GenerateJwtToken(result.Id, result.Username);
            return Ok(new { token = jwt });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel authModel)
        {
            _ = await _authService.RegisterUser(authModel);
            return Ok();
        }
    }
}
