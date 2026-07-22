using System.ComponentModel.DataAnnotations;

namespace WebApiAsistencia.DTOs
{
    public class SucursalRequestDto
    {
        [Required(ErrorMessage = "El nombre de la sucursal es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "La latitud es obligatoria.")]
        public decimal Latitud { get; set; }

        [Required(ErrorMessage = "La longitud es obligatoria.")]
        public decimal Longitud { get; set; }

        [Range(10, 1000, ErrorMessage = "El radio permitido debe estar entre 10 y 1000 metros.")]
        public int RadioPermitidoMetros { get; set; } = 100; // Valor por defecto (100m)
    }
}
