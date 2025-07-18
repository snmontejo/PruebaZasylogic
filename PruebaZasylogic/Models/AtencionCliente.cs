namespace PruebaZasylogic.Models
{
    public class AtencionCliente
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public int MotivoId { get; set; }
        public DateTime FechaAlta { get; set; }
    }
}