using WebApplication2.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using System.Net;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatabaseController : ControllerBase
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public DatabaseController(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        [HttpPost("SetConnectionString")]
        public IActionResult SetConnectionString([FromBody] ConnString model)
        {
            if (string.IsNullOrEmpty(model.ConnectionString))
            {
                return BadRequest("Connection string is required.");
            }

            string decodedConnectionString = WebUtility.UrlDecode(model.ConnectionString);
            _connectionStringProvider.SetConnectionString(decodedConnectionString);
            return Ok("Connection string set successfully.");
        }

        [HttpGet("GetConnectionString")]
        public string GetConnectionString()
        {
            return _connectionStringProvider.GetConnectionString();
        }
    }
}
