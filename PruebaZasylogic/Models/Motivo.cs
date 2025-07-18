namespace PruebaZasylogic.Models
{
    public class Motivo
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }

        public ICollection<AtencionCliente>? Atenciones { get; set; }
    }
}