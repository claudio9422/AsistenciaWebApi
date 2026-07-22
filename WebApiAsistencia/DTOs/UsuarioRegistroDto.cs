namespace WebApiAsistencia.DTOs
{
    public class UsuarioRegistroDto
    {
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string DocumentoIdentidad { get; set; } = null!;
        public int IdSucursal { get; set; }
        public int IdHorario { get; set; }
        public int IdRol { get; set; }
        public string PasswordClara { get; set; } = null!; // La clave que escribe el administrador para el empleado
    }
}
