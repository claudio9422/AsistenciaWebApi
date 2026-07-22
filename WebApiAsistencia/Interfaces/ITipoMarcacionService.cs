using WebApiAsistencia.DTOs;

namespace WebApiAsistencia.Interfaces
{
    public interface ITipoMarcacionService
    {
        Task<IEnumerable<TipoMarcacionResponseDto>> ListarTiposMarcacionesParaComboAsync();
    }
}
