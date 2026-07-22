using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiAsistencia.DTOs;
using WebApiAsistencia.Interfaces;

namespace WebApiAsistencia.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SucursalController : ControllerBase
    {
        private readonly ISucursalService _sucursalService;

        public SucursalController(ISucursalService sucursalService)
        {
            _sucursalService = sucursalService;
        }

        [HttpGet("Sucursales")]
        public async Task<IActionResult> GetSucursalesParaCombo()
        {
            try
            {
                var sucursales = await _sucursalService.ListarSucursalesParaComboAsync();
                return Ok(new { success = true, data = sucursales });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = "Ocurrió un error al listar las sucursales." });
            }
        }

        [HttpPost("Registrar")]
        public async Task<IActionResult> Registrar([FromBody] SucursalRequestDto sucursalRequest)
        {
            try
            {
                await _sucursalService.RegistrarSucursalAsync(sucursalRequest);
                return Ok(new { success = true, message = "La sucursal se ha registrado correctamente." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = "Ocurrió un error al registrar la sucursal." });
            }
        }
    }
}
