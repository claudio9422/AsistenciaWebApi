using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiAsistencia.DTOs;
using WebApiAsistencia.Interfaces;

namespace WebApiAsistencia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUsuarioService _usuarioService;

        public AuthController(IAuthService authService, IUsuarioService usuarioService)
        {
            _authService = authService;
            _usuarioService = usuarioService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.DocumentoIdentidad) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest(new { Success = false, Message = "Debe ingresar el DNI y la contraseña." });
            }

            var respuesta = await _authService.LoginAsync(dto);

            if (!respuesta.Success)
            {
                return Unauthorized(respuesta);
            }

            return Ok(respuesta);
        }

        [Authorize]
        [HttpPost("Registrar")]
        public async Task<IActionResult> Registrar([FromBody] UsuarioRegistroDto dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest(new { Success = false, Message = "Los datos del registro son requeridos." });

                // Evaluamos si el DNI tiene el formato mínimo esperado (Ej: 8 dígitos para Perú)
                if (string.IsNullOrWhiteSpace(dto.DocumentoIdentidad) || dto.DocumentoIdentidad.Length < 8)
                    return BadRequest(new { Success = false, Message = "El Documento de Identidad no es válido." });

                bool resultado = await _usuarioService.RegistrarUsuarioAsync(dto);

                if (!resultado)
                    return BadRequest(new { Success = false, Message = "No se pudo registrar el usuario. El DNI ya se encuentra registrado." });

                return Ok(new { success = true, message = "El usuario se ha registrado correctamente." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = "Ocurrió un error al registrar el usuario." });
            }
        }

    }
}
