using Microsoft.AspNetCore.Mvc;
using adisyon.Data;
using Microsoft.AspNetCore.Authorization;


namespace adisyon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "kasa")]
    public class CashController : ControllerBase
    {
        private readonly CashDAO _cashDAO;

        public CashController(CashDAO cashDAO)
        {
            _cashDAO = cashDAO;
        }
        
        // "Hazırlandı" durumundaki tüm siparişleri getirme
        [HttpGet("orders")]
        public async Task<IActionResult> GetReadyOrders()
        {
            var result = await _cashDAO.GetAllReadyOrdersAsync();

            if (result is string message)
            {
                // Eğer "Hazırlandı" sipariş yoksa, mesaj döndür
                return NotFound(message); // 404 - Bulunamadı
            }

            // "Hazırlandı" siparişler varsa, başarıyla listeyi döndür
            return Ok(result); // 200 - Başarı
        }
        
      
        
        // Belirtilen masa numarasındaki siparişlerin durumunu "Ödendi" olarak güncelleme
        [HttpPut("mark-paid/{tableNumber}")]
        public async Task<IActionResult> MarkOrdersAsPaidAsync(int tableNumber)
        {
            var result = await _cashDAO.MarkOrdersAsPaidAsync(tableNumber);

            if (result == Constants.TableEmpty)
            {
                return NotFound(result); // Masada güncellenebilir sipariş yoksa 404 döneriz
            }

            return Ok(result); // Siparişler "Ödendi" olarak işaretlendiyse, 200 ile döneriz
        }

    }
}
