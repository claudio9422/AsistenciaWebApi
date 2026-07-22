using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApiAsistencia.DTOs;
using WebApiAsistencia.Helpers;
using WebApiAsistencia.Interfaces;

namespace WebApiAsistencia.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AsistenciaController : ControllerBase
    {
        private readonly IAsistenciaService _asistenciaService;

        // 1. INYECCIÓN: .NET se encarga de pasarle el AsistenciaService automáticamente aquí
        public AsistenciaController(IAsistenciaService asistenciaService)
        {
            _asistenciaService = asistenciaService;
        }

        [HttpPost("Marcar")]
        [Consumes("multipart/form-data")]// Habilita la subida de archivos en Swagger/Postman
        public async Task<IActionResult> MarcarAsistencia([FromForm] AsistenciaRequestDto asistenciaRequest)
        {
            //var dniClaim = User.Claims.FirstOrDefault(c => c.Type == "DocumentoIdentidad")?.Value;
            
            var usuarioActivo = User.ObtenerUsuarioAutenticado();

            if (string.IsNullOrEmpty(usuarioActivo?.Dni))
            {
                return Unauthorized(new { Success = false, Message = "Token inválido o no contiene la identidad del usuario." });
            }

            if (asistenciaRequest.IdSucursal <= 0)
            {
                return BadRequest(new { success = false, message = "Debe seleccionar una sucursal válida." });
            }

            if (asistenciaRequest.IdTipoMarcacion <= 0)
            {
                return BadRequest(new { success = false, message = "Debe seleccionar un tipo de marcación válido." });
            }

            // 2. Filtro inicial del controlador: Validar que venga la foto
            if (asistenciaRequest.Foto == null || asistenciaRequest.Foto.Length == 0)
            {
                return BadRequest(new {success = false, message = "La foto es obligatoria."});
            }

            // Validar extensiones
            var extensionesPermitidas = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(asistenciaRequest.Foto.FileName).ToLower();

            if (!extensionesPermitidas.Contains(extension))
            {
                return BadRequest(new {success = false, message = "Las extensiones permitidas son .jpg, .jpeg y .png."});
            }

            try
            {
                // 3. LLAMADA AL SERVICIO (Aquí ocurre la magia)
                var resultado = await _asistenciaService.RegistrarAsistenciaAsync(asistenciaRequest, usuarioActivo.Dni);

                // 4. Retornar respuesta según lo que determinó la Base de Datos
                if (resultado.Success == 1)
                {
                    return Ok(new {success = true, message = resultado.Mensaje });
                }
                else
                {
                    // Si falló por rango de GPS, bloqueos u horarios, devolvemos 400 Bad Request
                    return BadRequest(new {success = false, message = resultado.Mensaje});
                }
            }
            catch (Exception ex)
            {
                // En caso de fallas imprevistas (permisos de disco, caída de BD, etc.)
                return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, error = ex.Message });
            }
        }

        [HttpGet]
        [Route("HistorialAsistencia")] // /api/Asistencia/Historial/{dni}
        public async Task<IActionResult> GetHistorialAsistencia([FromQuery] DateTime? fecha)
        {
            try
            {
                //var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var usuarioActivo = User.ObtenerUsuarioAutenticado();

                if (usuarioActivo == null)
                {
                    return Unauthorized(new { mensaje = "Usuario no autorizado o token no válido." });
                }

                var respuesta = await _asistenciaService.HistorialAsistenciaPorUsuarioAsync(usuarioActivo.IdUsuario, fecha);
                return Ok(new { success = true, data = respuesta });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, error = ex.Message });
            }
        }
    }
}
