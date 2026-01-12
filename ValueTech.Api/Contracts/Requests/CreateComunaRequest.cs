using System.ComponentModel.DataAnnotations;

namespace ValueTech.Api.Contracts.Requests
{
    public class CreateComunaRequest
    {
        [Required]
        public int IdRegion { get; set; }

        [Required]
        [StringLength(128)]
        public string Nombre { get; set; } = string.Empty;

        public decimal? Superficie { get; set; }
        public int? Poblacion { get; set; }
        public decimal? Densidad { get; set; }
    }
}
