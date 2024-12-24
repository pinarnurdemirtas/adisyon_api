using adisyon.Data;
using adisyon.Models;
using Microsoft.AspNetCore.Mvc;

namespace adisyon.Controllers;

[Route("api/[controller]")]
[ApiController]

public class MenuController : ControllerBase
{
    private readonly IMenuDAO _menuDao;

    public MenuController(IMenuDAO menuDao)
    {
        _menuDao = menuDao;
    }

    [HttpGet("get/all")]
    public async Task<ActionResult<IEnumerable<Products>>> GetUrun()
    {
        var products = await _menuDao.GetAll();
        return Ok(products);
    }

    [HttpGet("get/{id}")]
    public async Task<ActionResult<Products>> GetUrun(int id)
    {
        var product = await _menuDao.GetById(id);
        if (product == null) return NotFound();
        return Ok(product);
    }

    [HttpPost("post")]
    public async Task<ActionResult<Products>> PostUrun(Products product)
    {
        await _menuDao.Add(product);
        return CreatedAtAction(nameof(GetUrun), new { id = product.Id }, product);
    }

    [HttpPatch("patch/{id}")]
    public async Task<IActionResult> PutUrun(int id, Products product)
    {
        if (id != product.Id) return BadRequest();

        try
        {
            await _menuDao.Update(product);
        }
        catch (Exception)
        {
            if (!await _menuDao.Exists(id)) return NotFound();
            else throw;
        }
        return NoContent();
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteUrun(int id)
    {
        var exists = await _menuDao.Exists(id);
        if (!exists) return NotFound();

        await _menuDao.Delete(id);
        return NoContent();
    }
}