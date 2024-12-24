using adisyon.Data;
using adisyon.Models;
using Microsoft.AspNetCore.Mvc;

namespace adisyon.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TablesController : ControllerBase
    {
        private readonly TablesDAO _tablesDAO;

        public TablesController(TablesDAO tablesDAO)
        {
            _tablesDAO = tablesDAO;
        }

        [HttpPost("post")]
        public async Task<IActionResult> AddTable([FromBody] Tables table)
        {
            if (table == null)
            {
                return BadRequest(Message.TableNotFound);
            }

            var newTable = await _tablesDAO.AddTableAsync(table);
            return CreatedAtAction(nameof(GetTableByNumber), new { tableNumber = newTable.Table_number }, newTable);
        }

        [HttpDelete("delete/{tableNumber}")]
        public async Task<IActionResult> DeleteTable(int tableNumber)
        {
            var result = await _tablesDAO.DeleteTableAsync(tableNumber);
            if (!result)
            {
                return NotFound(Message.TableNotFound);
            }

            return NoContent(); 
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetAllTables()
        {
            var tables = await _tablesDAO.GetAllTablesAsync();
            return Ok(tables);
        }

        [HttpGet("{tableNumber}")]
        public async Task<IActionResult> GetTableByNumber(int tableNumber)
        {
            var table = await _tablesDAO.GetTableByNumberAsync(tableNumber);
            if (table == null)
            {
                return NotFound(Message.TableNotFound);
            }

            return Ok(table);
        }
    }
}