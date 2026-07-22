using WebApiAsistencia.DTOs;

namespace WebApiAsistencia.Interfaces
{
    public interface ISucursalService
    {
        Task RegistrarSucursalAsync(SucursalRequestDto sucursalRequest);
        Task<IEnumerable<SucursalResponseDto>> ListarSucursalesParaComboAsync();
    }
}
