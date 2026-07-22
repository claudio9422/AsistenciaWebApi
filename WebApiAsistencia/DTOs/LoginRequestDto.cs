namespace WebApiAsistencia.DTOs
{
    public class LoginRequestDto
    {
        public string DocumentoIdentidad { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
