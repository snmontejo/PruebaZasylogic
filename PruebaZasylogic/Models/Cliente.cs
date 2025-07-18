namespace PruebaZasylogic.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public string Sexo { get; set; }

        public ICollection<AtencionCliente>? Atenciones { get; set; }
    }
}