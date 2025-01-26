using eCommerce.Core.DTO;
using eCommerce.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers
{
    [Route("api/[controller]")] //api/auth
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsersService _userService;
        public AuthController(IUsersService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")] //POST api/auth/register
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            if (registerRequest == null)
            {
                return BadRequest("Invalid registeration data");
            }

            AuthenticationResponse? response = await _userService.Register(registerRequest);

            if (response == null || response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            if (loginRequest == null)
            {
                return BadRequest("Invalid login data");
            }

            AuthenticationResponse? response = await _userService.Login(loginRequest);

            if(response == null || response.Success == false)
            {
                return Unauthorized(response);
            }

            return Ok(response);
            
        }
    }
}
