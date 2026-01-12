using System.ComponentModel.DataAnnotations;

namespace ValueTech.Web.Models
{
    public class ComunaFormViewModel
    {
        public int IdComuna { get; set; } // 0 for Create

        [Required]
        public int IdRegion { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(128)]
        public string Nombre { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "La superficie debe ser positiva.")]
        public decimal? Superficie { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "La población debe ser positiva.")]
        public int? Poblacion { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "La densidad debe ser positiva.")]
        public decimal? Densidad { get; set; }
    }
}
