using adisyon.Data;
using adisyon.Models;
using Microsoft.EntityFrameworkCore;

namespace adisyon.Data.Repositories;

public class MenuDAO
{
    private readonly AdisyonDbContext _context;

    public MenuDAO(AdisyonDbContext context)
    {
        _context = context;
    }

    // Tüm ürünleri getir
    public async Task<IEnumerable<Products>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }

    // Belirli bir ID'ye sahip ürünü getir
    public async Task<Products?> GetByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    // Yeni ürün ekle
    public async Task AddAsync(Products product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
    }

    // Mevcut ürünü güncelle
    public async Task UpdateAsync(Products product)
    {
        _context.Entry(product).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    // Ürünü sil
    public async Task DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }

    // Ürün var mı kontrolü
    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Products.AnyAsync(e => e.Id == id);
    }
}