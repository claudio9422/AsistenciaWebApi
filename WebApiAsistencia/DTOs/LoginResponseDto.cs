namespace WebApiAsistencia.DTOs
{
    public class LoginResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public string? Token { get; set; }
        public bool EsPrimerIngreso { get; set; }
        public bool PermiteCambioClave { get; set; }
        public string? NombreCompleto { get; set; }
        public string? Rol { get; set; }
    }
}
