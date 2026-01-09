namespace ValueTech.Data.Models
{
    public class Comuna
    {
        public int IdComuna { get; set; }
        public int IdRegion { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? InformacionAdicional { get; set; }
    }
}
