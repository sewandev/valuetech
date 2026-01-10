using System.ComponentModel.DataAnnotations;

namespace ValueTech.Api.Contracts.Requests
{
    public class UpdateComunaRequest
    {
        public int IdComuna { get; set; }
        public int IdRegion { get; set; }
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La Superficie es obligatoria.")]
        public decimal? Superficie { get; set; }

        [Required(ErrorMessage = "La Población es obligatoria.")]
        public int? Poblacion { get; set; }

        [Required(ErrorMessage = "La Densidad es obligatoria.")]
        public decimal? Densidad { get; set; }
    }
}
