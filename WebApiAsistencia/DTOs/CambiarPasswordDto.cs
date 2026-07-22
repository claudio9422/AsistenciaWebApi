namespace WebApiAsistencia.DTOs
{
    public class CambiarPasswordDto
    {
        public string DocumentoIdentidad { get; set; } = null!;
        public string PasswordActual { get; set; } = null!;
        public string PasswordNueva { get; set; } = null!;
    }
}
