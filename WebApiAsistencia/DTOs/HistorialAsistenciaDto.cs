namespace WebApiAsistencia.DTOs
{
    public class HistorialAsistenciaDto
    {
        public int IdMarcacion { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Hora { get; set; } = string.Empty;
        public string Fecha { get; set; } = string.Empty;
        public string Sucursal { get; set; } = string.Empty;
        public string Foto { get; set; } = string.Empty;
    }
}
