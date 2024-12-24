using adisyon.Data.Repositories;
using adisyon.Models;
using Microsoft.AspNetCore.Mvc;

namespace adisyon.Controllers;

[Route("api/[controller]")]
[ApiController]

public class MenuController : ControllerBase
{
    private readonly MenuDAO _menuDao;

    public MenuController(MenuDAO menuDao)
    {
        _menuDao = menuDao;
    }

    [HttpGet("get/all")]
    public async Task<ActionResult<IEnumerable<Products>>> GetUrun()
    {
        var products = await _menuDao.GetAllAsync();
        return Ok(products);
    }

    [HttpGet("get/{id}")]
    public async Task<ActionResult<Products>> GetUrun(int id)
    {
        var product = await _menuDao.GetByIdAsync(id);
        if (product == null) return NotFound();
        return Ok(product);
    }

    [HttpPost("post")]
    public async Task<ActionResult<Products>> PostUrun(Products product)
    {
        await _menuDao.AddAsync(product);
        return CreatedAtAction(nameof(GetUrun), new { id = product.Id }, product);
    }

    [HttpPatch("patch/{id}")]
    public async Task<IActionResult> PutUrun(int id, Products product)
    {
        if (id != product.Id) return BadRequest();

        try
        {
            await _menuDao.UpdateAsync(product);
        }
        catch (Exception)
        {
            if (!await _menuDao.ExistsAsync(id)) return NotFound();
            else throw;
        }
        return NoContent();
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteUrun(int id)
    {
        var exists = await _menuDao.ExistsAsync(id);
        if (!exists) return NotFound();

        await _menuDao.DeleteAsync(id);
        return NoContent();
    }
}