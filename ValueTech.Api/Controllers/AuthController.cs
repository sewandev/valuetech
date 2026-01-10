using Microsoft.AspNetCore.Mvc;
using ValueTech.Api.Contracts.Requests;
using ValueTech.Data.Interfaces;
using ValueTech.Data.Models; // For Auditoria

namespace ValueTech.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repository;
        private readonly IAuditoriaRepository _auditRepository; // Added this field

        public AuthController(IAuthRepository repository, IAuditoriaRepository auditRepository) // Modified constructor
        {
            _repository = repository;
            _auditRepository = auditRepository; // Assigned injected repository
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _repository.ValidateUserAsync(request.Username, request.Password);
            string ip = "Unknown";
            if (Request.Headers.TryGetValue("X-Audit-IP", out var headerIp))
            {
                ip = headerIp.ToString();
            }
            else
            {
                ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            }
            
            if (user == null)
            {
                await _auditRepository.AddAsync(new Auditoria 
                { 
                    Usuario = request.Username, 
                    IpAddress = ip, 
                    Accion = "LOGIN_FAILED", 
                    Detalle = "Credenciales inválidas" 
                });
                return Unauthorized(new { error = "Credenciales inválidas" });
            }
            await _auditRepository.AddAsync(new Auditoria 
            { 
                Usuario = user.Username, 
                IpAddress = ip, 
                Accion = "LOGIN_SUCCESS", 
                Detalle = "Inicio de sesión exitoso" 
            });

            return Ok(new { 
                id = user.IdUsuario, 
                username = user.Username, 
                message = "Login exitoso" 
            });
        }
    }
}
