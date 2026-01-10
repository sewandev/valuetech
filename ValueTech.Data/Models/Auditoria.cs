namespace ValueTech.Data.Models
{
    public class Auditoria
    {
        public int IdAuditoria { get; set; }
        public DateTime Fecha { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
        public string Accion { get; set; } = string.Empty;
        public string Detalle { get; set; } = string.Empty;
    }
}
