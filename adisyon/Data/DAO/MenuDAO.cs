using adisyon.Models;
using Microsoft.EntityFrameworkCore;

namespace adisyon.Data
{
    public interface IMenuDAO
    {
        Task<IEnumerable<Products>> GetAll();
        Task<Products?> GetById(int id);
        Task Add(Products product);
        Task Update(Products product);
        Task Delete(int id);
        Task<bool> Exists(int id);
    }
    public class MenuDAO : IMenuDAO
    {
        private readonly AdisyonDbContext _context;

        public MenuDAO(AdisyonDbContext context)
        {
            _context = context;
        }

        // Tüm ürünleri getir
        public async Task<IEnumerable<Products>> GetAll()
        {
            return await _context.Products.ToListAsync();
        }

        // Belirli bir ID'ye sahip ürünü getir
        public async Task<Products?> GetById(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        // Yeni ürün ekle
        public async Task Add(Products product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        // Mevcut ürünü güncelle
        public async Task Update(Products product)
        {
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        // Ürünü sil
        public async Task Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        // Ürün var mı kontrolü
        public async Task<bool> Exists(int id)
        {
            return await _context.Products.AnyAsync(e => e.Id == id);
        }
    }
}
