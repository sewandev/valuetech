using System.ComponentModel.DataAnnotations;

namespace ValueTech.Web.Models
{
    public class RegionFormViewModel
    {
        public int IdRegion { get; set; } // 0 for Create

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(64, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 64 caracteres.")]
        public string Nombre { get; set; } = string.Empty;
    }
}
