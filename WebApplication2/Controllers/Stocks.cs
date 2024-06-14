using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StocksController : ControllerBase
    {
        private readonly DatabaseController _databaseController;

        public StocksController(DatabaseController databaseController)
        {
            _databaseController = databaseController;
        }

        [HttpGet("ExecuteStoredProcedure")]
        public async Task<IActionResult> ExecuteStoredProcedure(string param1, string param2)
        {
            if (string.IsNullOrEmpty(param1) || string.IsNullOrEmpty(param2))
            {
                return BadRequest("Parameters cannot be null or empty");
            }

            var connectionString = _databaseController.GetConnectionString();

            try
            {
                var result = await ExecuteStoredProcedureAsync(connectionString, param1, param2);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        private async Task<List<Stock>> ExecuteStoredProcedureAsync(string connectionString, string dbName, string tablePattern)
        {
            var stocks = new List<Stock>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand("getInventory", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@schem", dbName);
                    command.Parameters.AddWithValue("@patrn", tablePattern);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            stocks.Add(new Stock
                            {
                                StkSCode = reader.GetString("stk_scode"),
                                StkItemNo = reader.GetString("stk_itemno"),
                                StkLocation = reader.GetString("stk_location"),
                                StkSubLocation = reader.GetString("stk_sublocation"),
                                StkSourceUOM = reader.GetString("stk_sourceuom"),
                                StkLotNo = reader.GetString("stk_lotno"),
                                StkExpiration = reader.IsDBNull("stk_expiration") ? (DateTime?)null : reader.GetDateTime("stk_expiration"),
                                StkAvailableQty = reader.GetInt32("stk_availableQty"),
                                StkActualQty = reader.GetInt32("stk_actualQty"),
                                StkLogDateTime = reader.IsDBNull("stk_logdatetime") ? (DateTime?)null : reader.GetDateTime("stk_logdatetime")
                            });
                        }
                    }
                }
            }

            return stocks;
        }
    }
}
