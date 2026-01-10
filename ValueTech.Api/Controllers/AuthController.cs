using Microsoft.AspNetCore.Mvc;
using ValueTech.Api.Contracts.Requests;
using ValueTech.Data.Interfaces;

namespace ValueTech.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repository;

        public AuthController(IAuthRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _repository.ValidateUserAsync(request.Username, request.Password);
            
            if (user == null)
            {
                return Unauthorized(new { error = "Credenciales inválidas" });
            }
            return Ok(new { 
                id = user.IdUsuario, 
                username = user.Username, 
                message = "Login exitoso" 
            });
        }
    }
}
