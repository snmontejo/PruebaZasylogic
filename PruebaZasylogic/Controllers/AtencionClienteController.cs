using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PruebaZasylogic.Models;
using System.Data;

namespace PruebaZasylogic.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AtencionClienteController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AtencionClienteController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("registrar")]
        public IActionResult RegistrarAtencion([FromBody] AtencionCliente atencion)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("sp_InsertarAtencion", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ClienteId", atencion.ClienteId);
                command.Parameters.AddWithValue("@MotivoId", atencion.MotivoId);

                connection.Open();
                command.ExecuteNonQuery();
            }

            return Ok(new { mensaje = "Atención registrada correctamente (SP)." });
        }

        [HttpPost("verificar")]
        public IActionResult VerificarAtencionHoy([FromBody] string celular)
        {
            var resultados = new List<dynamic>();
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("sp_VerificarAtencionHoy", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Celular", celular);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    resultados.Add(new
                    {
                        Id = reader["Id"],
                        Nombre = reader["Nombre"],
                        Apellidos = reader["Apellidos"],
                        Motivo = reader["Motivo"],
                        FechaAlta = reader["Fecha_Alta"]
                    });
                }
            }

            return Ok(resultados);
        }
        [HttpPost("listado")]
        public IActionResult ListadoAtenciones([FromBody] FiltroFecha filtro)
        {
            var lista = new List<object>();
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            using var command = new SqlCommand("sp_ListarAtencionesPorFechas", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Desde", filtro.Desde);
            command.Parameters.AddWithValue("@Hasta", filtro.Hasta);

            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new
                {
                    nombre = reader["Nombre"],
                    motivo = reader["Motivo"],
                    fecha = Convert.ToDateTime(reader["Fecha_Alta"]).ToString("yyyy-MM-dd")
                });
            }

            return Ok(lista);
        }

        public class FiltroFecha
        {
            public DateTime Desde { get; set; }
            public DateTime Hasta { get; set; }
        }
    }
}