namespace WebApiAsistencia.DTOs
{
    public class UsuarioSesionDto
    {
        public int IdUsuario { get; set; }
        public string Dni { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
    }
}
