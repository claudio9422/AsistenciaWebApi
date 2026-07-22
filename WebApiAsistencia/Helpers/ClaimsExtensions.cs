using System.Security.Claims;
using WebApiAsistencia.DTOs;

namespace WebApiAsistencia.Helpers
{
    public static class ClaimsExtensions
    {
        public static UsuarioSesionDto? ObtenerUsuarioAutenticado(this ClaimsPrincipal user)
        {
            if(user?.Identity?.IsAuthenticated != true)
            {
                return null;
            }

            var idClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(!int.TryParse(idClaim, out int idUsuario))
            {
                return null;// No se pudo obtener el id del usuario
            }

            return new UsuarioSesionDto
            {
                IdUsuario = idUsuario,
                Dni = user.Claims.FirstOrDefault(c => c.Type == "DocumentoIdentidad")?.Value ?? string.Empty,
                Email = user.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty,
                Nombre = user.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty,
                Rol = user.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty
            };
        }
    }
}
