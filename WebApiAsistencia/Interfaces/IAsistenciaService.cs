using WebApiAsistencia.DTOs;

namespace WebApiAsistencia.Interfaces
{
    public interface IAsistenciaService
    {
        Task<DatabaseResultDto> RegistrarAsistenciaAsync(AsistenciaRequestDto asistenciaRequest, string DocumentoEntidad);
        Task<List<HistorialAsistenciaDto>> HistorialAsistenciaPorUsuarioAsync(int IdUsuario, DateTime? fechaFiltro = null);
    }
}
