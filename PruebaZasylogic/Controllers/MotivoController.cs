using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PruebaZasylogic.Models;
using System.Data;

namespace PruebaZasylogic.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MotivoController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public MotivoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult InsertarMotivo([FromBody] Motivo motivo)
        {
            using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            using SqlCommand command = new SqlCommand("sp_InsertarMotivo", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Descripcion", motivo.Descripcion);

            connection.Open();
            command.ExecuteNonQuery();

            return Ok(new { mensaje = "Motivo insertado correctamente (SP)." });
        }

        [HttpGet]
        public IActionResult ListarMotivos()
        {
            var motivos = new List<Motivo>();
            using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            using SqlCommand command = new SqlCommand("sp_ListarMotivos", connection);
            command.CommandType = CommandType.StoredProcedure;

            connection.Open();
            using SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                motivos.Add(new Motivo
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Descripcion = reader["Descripcion"].ToString()
                });
            }

            return Ok(motivos);
        }
    }
}