using Azure.Core;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Globalization;
using WebApiAsistencia.Data;
using WebApiAsistencia.DTOs;
using WebApiAsistencia.Helpers;
using WebApiAsistencia.Interfaces;

namespace WebApiAsistencia.Services
{
    public class AsistenciaService : IAsistenciaService
    {
        private readonly DbAsistenciaContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AsistenciaService(DbAsistenciaContext context, IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<HistorialAsistenciaDto>> HistorialAsistenciaPorUsuarioAsync(int IdUsuario, DateTime? fechaFiltro = null)
        {
            var culturaEspanol = new CultureInfo("es-ES");

            DateTime fechaBase = fechaFiltro?.Date ?? DateTime.Today;
            
            DateTime fechaInicio = fechaBase.Date;
            DateTime limiteSiguienteDia = fechaBase.AddDays(1);

            var request = _httpContextAccessor.HttpContext?.Request;
            string baseUrl = request != null ? $"{request.Scheme}://{request.Host}{request.PathBase}" : string.Empty;

            var marcaciones = await _context.Marcaciones
                .AsNoTracking()
                .Where(m => m.IdUsuario == IdUsuario &&
                       m.EsValida == true &&
                       m.FechaHoraRegistro >= fechaInicio && 
                       m.FechaHoraRegistro < limiteSiguienteDia)
                .OrderByDescending(m => m.FechaHoraRegistro)
                .Select(m => new HistorialAsistenciaDto
                {
                    IdMarcacion = m.IdMarcacion,
                    Tipo = m.IdTipoMarcacionNavigation!.Nombre,
                    Hora = m.FechaHoraRegistro.ToString("hh:mm tt", culturaEspanol).ToLower()
                            .Replace("am", "a.m.")
                            .Replace("pm", "p.m."),
                    Fecha = m.FechaHoraRegistro.ToString("dddd, dd 'de' MMMM 'de' yyyy", culturaEspanol).CapitalizarPrimeraLetra(),
                    Sucursal = m.IdSucursalNavigation!.Nombre,
                    Foto = !string.IsNullOrEmpty(m.RutaFoto) ? $"{baseUrl}{m.RutaFoto}" : string.Empty
                }).ToListAsync();

            return marcaciones;
        }

        public async Task<DatabaseResultDto> RegistrarAsistenciaAsync(AsistenciaRequestDto asistenciaRequest, string DocumentoEntidad)
        {
            var resultado = new DatabaseResultDto();

            string extensionArchivo = Path.GetExtension(asistenciaRequest.Foto.FileName).ToLower();
            string nombreArchivo = $"{DocumentoEntidad}_{DateTime.Now:yyyyMMddHHmmss}{extensionArchivo}";
            
            string rutaRelativaDb = $"/uploads/fotos/{DateTime.Now.Year}/{nombreArchivo}";
            
            using ( var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                using (var command = new SqlCommand("sp_RegistrarAsistencia", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@DocumentoIdentidad",DocumentoEntidad);
                    command.Parameters.AddWithValue("@LatitudUsuario", asistenciaRequest.Latitud);
                    command.Parameters.AddWithValue("@LongitudUsuario", asistenciaRequest.Longitud);
                    command.Parameters.AddWithValue("@RutaFoto", rutaRelativaDb);
                    command.Parameters.AddWithValue("@IdSucursal", asistenciaRequest.IdSucursal);
                    command.Parameters.AddWithValue("@IdTipoMarcacion", asistenciaRequest.IdTipoMarcacion);

                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            resultado.Success = Convert.ToInt32(reader["Success"]);
                            resultado.Mensaje = reader["Message"].ToString()!;
                        }
                    }
                }
            }

            // 3. CONTROLAR EL GUARDADO FÍSICO SEGÚN EL RESULTADO DE LA BD
            if (resultado.Success == 1)
            {
                // 1. Lógica de guardado de archivo físico
                string carpetaUploads = Path.Combine(_environment.WebRootPath, "uploads", "fotos", DateTime.Now.Year.ToString());
                if (!Directory.Exists(carpetaUploads))
                {
                    Directory.CreateDirectory(carpetaUploads);
                }

                string rutaCompletaFisica = Path.Combine(carpetaUploads, nombreArchivo);

                using (var stream = new FileStream(rutaCompletaFisica, FileMode.Create))
                {
                    await asistenciaRequest.Foto.CopyToAsync(stream);
                }
            }


            return resultado;
        }
    }
}
