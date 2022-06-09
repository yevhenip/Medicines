using Medicines.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Medicines.Controllers;

[ApiController]
[Route("[controller]")]
public class MedicinesController : ControllerBase
{
    private readonly MedicinesContext _context;

    public MedicinesController(MedicinesContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_context.Medicines.FromSqlRaw(@"select * from Medicines"));
    }

    [HttpGet("{id:guid}")]
    public IActionResult Get(Guid id)
    {
        return Ok(_context.Medicines.FromSqlRaw(@"select * from Medicines where Id = {0}", id));
    }

    [HttpPost]
    public async Task<IActionResult> Create(Medicine medicine)
    {
        var id = Guid.NewGuid();
        medicine.Id = id;

        _context.Medicines.FromSqlRaw(@"insert into Medicines(Name, Id, Manufacturer, Price) 
                                                    values({0}, {1}, {2}, {3})",
            medicine.Name, medicine.Id, medicine.Manufacturer, medicine.Price);

        await _context.SaveChangesAsync();

        return Ok(id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, Medicine medicine)
    {
        var model = _context.Medicines.FromSqlRaw(@"select * from Medicines where Id = {0}", id);
        if (model.FirstOrDefault() is null)
            return BadRequest();

        medicine.Id = id;

        _context.Medicines.FromSqlRaw("update Medicines set Price = {0}, Name = {1}, Manufacturer = {2} where Id = {3}",
            medicine.Price, medicine.Name, medicine.Manufacturer, medicine.Id);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var model = _context.Medicines.FromSqlRaw(@"select * from Medicines where Id = {0}", id);
        if (model.FirstOrDefault() is null)
            return BadRequest();

        _context.Medicines.FromSqlRaw("delete from Medicines where Id = {0}", id);
        await _context.SaveChangesAsync();

        return Ok();
    }
}