using System.ComponentModel.DataAnnotations;

namespace WebApiAsistencia.DTOs
{
    public class AsistenciaRequestDto
    {
        //[Required(ErrorMessage = "El documento de indentidad es requerido")]
        //[StringLength(8, MinimumLength = 8, ErrorMessage = "El documento de identidad debe tener 8 caracteres")]
        //public string DocumentoEntidad { get; set; } = null!;

        [Required(ErrorMessage = "La latitud es requerido")]
        [Range(-90, 90, ErrorMessage = "La latitud debe estar entre -90 y 90")]
        public decimal Latitud { get; set; }

        [Required(ErrorMessage = "La longitud es requerido")]
        [Range(-180, 180, ErrorMessage = "La longitud debe estar entre -180 y 180")]
        public decimal Longitud { get; set; }

        [Required(ErrorMessage = "La foto de la marcación es requerido")]
        public IFormFile Foto { get; set; } = null!;

        public int IdSucursal { get; set; }
        public int IdTipoMarcacion { get; set; }
    }
}
