using adisyon.Models;
using Microsoft.EntityFrameworkCore;

namespace adisyon.Data
{
    public interface ITablesDAO
    {
        Task<Tables> AddTable(Tables table);
        Task<bool> DeleteTable(int tableNumber);
        Task<IEnumerable<Tables>> GetAllTables();
        Task<Tables> GetTableByNumber(int tableNumber);
    }
    public class TablesDAO : ITablesDAO
    {
        private readonly AdisyonDbContext _context;

        public TablesDAO(AdisyonDbContext context)
        {
            _context = context;
        }

        // Yeni masa ekle
        public async Task<Tables> AddTable(Tables table)
        {
            await _context.Tables.AddAsync(table);
            await _context.SaveChangesAsync();
            return table;
        }

        // Masayı bul
        public async Task<Tables> GetTableByNumber(int tableNumber)
        {
            return await _context.Tables.FindAsync(tableNumber);
        }

        // Tüm masaları getir
        public async Task<IEnumerable<Tables>> GetAllTables()
        {
            return await _context.Tables.ToListAsync();
        }

        // Masa sil
        public async Task<bool> DeleteTable(int tableNumber)
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