using System.ComponentModel.DataAnnotations;

namespace ValueTech.Api.Contracts.Requests
{
    public class CreateRegionRequest
    {
        [Required]
        [StringLength(64, MinimumLength = 3)]
        public string Nombre { get; set; } = string.Empty;
    }
}
