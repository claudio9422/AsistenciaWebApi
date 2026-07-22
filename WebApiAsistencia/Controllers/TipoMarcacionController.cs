using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiAsistencia.Interfaces;

namespace WebApiAsistencia.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TipoMarcacionController : ControllerBase
    {
        private readonly ITipoMarcacionService _tipoMarcacionService;
        public TipoMarcacionController(ITipoMarcacionService tipoMarcacionService)
        {
            _tipoMarcacionService = tipoMarcacionService;
        }

        [HttpGet("TiposMarcacion")]
        public async Task<IActionResult> GetTiposMarcacionesParaCombo()
        {
            try
            {
                var tiposMarcaciones = await _tipoMarcacionService.ListarTiposMarcacionesParaComboAsync();
                return Ok(new { success = true, data = tiposMarcaciones });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = "Ocurrió un error al listar los tipos de marcación." });
            }
        }
    }
}
