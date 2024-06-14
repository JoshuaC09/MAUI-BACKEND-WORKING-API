using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatabaseController : ControllerBase
    {
        private readonly MyDbContextFactory _dbContextFactory;
        private static string _connectionString;
        private readonly IConfiguration _configuration;

        public DatabaseController(MyDbContextFactory dbContextFactory, IConfiguration configuration)
        {
            _dbContextFactory = dbContextFactory;
            _configuration = configuration;
        }

        [HttpPost("SetConnectionString")]
        public IActionResult SetConnectionString([FromBody] ConnectionStringModel model)
        {
            if (string.IsNullOrEmpty(model.ConnectionString))
            {
                return BadRequest("Connection string is required.");
            }

            try
            {
                // Log the received connection string
                Console.WriteLine($"Received Connection String: {model.ConnectionString}");
                string decodedConnectionString = System.Net.WebUtility.UrlDecode(model.ConnectionString);
                Console.WriteLine($"Decoded Connection String: {decodedConnectionString}");

                // Append the required options to handle zero DateTime values
                _connectionString = $"{decodedConnectionString};Convert Zero Datetime=True;Allow Zero Datetime=True";
                return Ok("Connection string set successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Exception in SetConnectionString: {ex.Message}");
                return StatusCode(500, "An error occurred while setting the connection string.");
            }
        }



        [HttpGet("GetConnectionString")]
        public string GetConnectionString()
        {
            return _connectionString ?? _configuration.GetConnectionString("MyDbConnectionStrings");
        }
    }

    public class ConnectionStringModel
    {
        public string ConnectionString { get; set; }
    }
}
