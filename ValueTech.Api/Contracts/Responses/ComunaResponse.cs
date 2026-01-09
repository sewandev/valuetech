namespace ValueTech.Api.Contracts.Responses
{
    public class ComunaResponse
    {
        public int IdComuna { get; set; }
        public int IdRegion { get; set; }
        public string Nombre { get; set; } = string.Empty;
        
        public decimal Superficie { get; set; }
        public int Poblacion { get; set; }
        public decimal Densidad { get; set; }
    }
}
