using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PruebaZasylogic.Models;
using System.Data;

namespace PruebaZasylogic.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ClienteController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult InsertarCliente([FromBody] Cliente cliente)
        {
            using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            using SqlCommand command = new SqlCommand("sp_InsertarCliente", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Nombre", cliente.Nombre);
            command.Parameters.AddWithValue("@Apellidos", cliente.Apellidos);
            command.Parameters.AddWithValue("@Celular", cliente.Celular);
            command.Parameters.AddWithValue("@Email", cliente.Email);
            command.Parameters.AddWithValue("@Sexo", cliente.Sexo);

            connection.Open();
            command.ExecuteNonQuery();

            return Ok(new { mensaje = "Cliente insertado correctamente (SP)." });
        }

        [HttpGet]
        public IActionResult ListarClientes()
        {
            var clientes = new List<Cliente>();
            using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            using SqlCommand command = new SqlCommand("sp_ListarClientes", connection);
            command.CommandType = CommandType.StoredProcedure;

            connection.Open();
            using SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                clientes.Add(new Cliente
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Nombre = reader["Nombre"].ToString(),
                    Apellidos = reader["Apellidos"].ToString(),
                    Celular = reader["Celular"].ToString(),
                    Email = reader["Email"].ToString(),
                    Sexo = reader["Sexo"].ToString()
                });
            }

            return Ok(clientes);
        }
    }
}