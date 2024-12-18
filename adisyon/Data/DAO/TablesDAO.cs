using adisyon.Models;
using Microsoft.EntityFrameworkCore;

namespace adisyon.Data
{
    public class TablesDAO
    {
        private readonly AdisyonDbContext _context;

        public TablesDAO(AdisyonDbContext context)
        {
            _context = context;
        }

        // Yeni masa eklemek için metod
        public async Task<Tables> AddTable(Tables table)
        {
            // Yeni masayı ekle ve kaydet
            await _context.Tables.AddAsync(table);
            await _context.SaveChangesAsync();

            return table;
        }

        // Masa silme işlemi
        public async Task<bool> DeleteTable(int tableNumber)
        {
            var table = await _context.Tables.FindAsync(tableNumber);

            if (table == null)
            {
                return false; // Masa bulunamadı
            }

            _context.Tables.Remove(table);
            await _context.SaveChangesAsync();

            return true; // Masa başarıyla silindi
        }

        // Tüm masaları getiren metod
        public async Task<List<Tables>> GetAllTables()
        {
            return await _context.Tables.ToListAsync();
        }

        // Belirli bir masayı getiren metod
        public async Task<Tables> GetTableByNumber(int tableNumber)
        {
            return await _context.Tables.FindAsync(tableNumber);
        }
    }
}