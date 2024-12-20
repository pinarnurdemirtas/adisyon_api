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
        public async Task<Tables> AddTableAsync(Tables table)
        {
            await _context.Tables.AddAsync(table);
            await _context.SaveChangesAsync();
            return table;
        }

        // Masa bulmak için metod
        public async Task<Tables> GetTableByNumberAsync(int tableNumber)
        {
            return await _context.Tables.FindAsync(tableNumber);
        }

        // Tüm masaları getiren metod
        public async Task<List<Tables>> GetAllTablesAsync()
        {
            return await _context.Tables.ToListAsync();
        }

        // Masa silmek için metod
        public async Task<bool> DeleteTableAsync(int tableNumber)
        {
            var table = await _context.Tables.FindAsync(tableNumber);
            if (table == null)
            {
                return false;
            }

            _context.Tables.Remove(table);
            await _context.SaveChangesAsync();
            return true;
        }

        // Değişiklikleri kaydet
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}