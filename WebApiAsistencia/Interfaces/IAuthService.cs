using WebApiAsistencia.DTOs;

namespace WebApiAsistencia.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequest);
    }
}
