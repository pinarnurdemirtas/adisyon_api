using adisyon.Data;
using adisyon.Models;
using Microsoft.AspNetCore.Mvc;

namespace adisyon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TablesController : ControllerBase
    {
        private readonly TablesDAO _tablesDAO;

        public TablesController(TablesDAO tablesDAO)
        {
            _tablesDAO = tablesDAO;
        }

        // Yeni masa eklemek için POST metodunu kullanıyoruz
        [HttpPost("add-table")]
        public async Task<IActionResult> AddTable([FromBody] Tables table)
        {
            if (table == null || string.IsNullOrEmpty(table.Table_status))
            {
                return BadRequest("Geçersiz masa verisi.");
            }

            var newTable = await _tablesDAO.AddTable(table);

            return CreatedAtAction(nameof(GetTableByNumber), new { tableNumber = newTable.Table_number }, newTable);
        }

        // Masa silmek için DELETE metodunu kullanıyoruz
        [HttpDelete("delete-table/{tableNumber}")]
        public async Task<IActionResult> DeleteTable(int tableNumber)
        {
            var success = await _tablesDAO.DeleteTable(tableNumber);

            if (!success)
            {
                return NotFound("Masa bulunamadı.");
            }

            return NoContent(); // Başarıyla silindi
        }

        // Tüm masaları listeleyen metod
        [HttpGet("all-tables")]
        public async Task<IActionResult> GetAllTables()
        {
            var tables = await _tablesDAO.GetAllTables();
            return Ok(tables);
        }

        // Belirli bir masayı getiren metod
        [HttpGet("table/{tableNumber}")]
        public async Task<IActionResult> GetTableByNumber(int tableNumber)
        {
            var table = await _tablesDAO.GetTableByNumber(tableNumber);

            if (table == null)
            {
                return NotFound("Masa bulunamadı.");
            }

            return Ok(table);
        }
    }
}