using WebApiAsistencia.DTOs;

namespace WebApiAsistencia.Interfaces
{
    public interface IUsuarioService
    {
        Task<bool> RegistrarUsuarioAsync(UsuarioRegistroDto dto);
    }
}
