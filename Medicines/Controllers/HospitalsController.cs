using Medicines.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Medicines.Controllers;

[ApiController]
[Route("[controller]")]
public class HospitalsController : ControllerBase
{
    private readonly MedicinesContext _context;

    public HospitalsController(MedicinesContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _context.Hospitals.ToListAsync());
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        return Ok(await _context.Hospitals.FirstOrDefaultAsync(h => h.Id == id));
    }

    [HttpPost]
    public async Task<IActionResult> Create(Hospital hospital)
    {
        var id = Guid.NewGuid();
        hospital.Id = id;

        await _context.Hospitals.AddAsync(hospital);
        await _context.SaveChangesAsync();

        return Ok(id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, Hospital hospital)
    {
        var model = await _context.Hospitals.FirstOrDefaultAsync(h => h.Id == id);
        if (model is null)
            return BadRequest();

        hospital.Id = id;

        _context.Hospitals.Update(hospital);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var model = await _context.Hospitals.FirstOrDefaultAsync(h => h.Id == id);
        if (model is null)
            return BadRequest();

        _context.Hospitals.Remove(model);
        await _context.SaveChangesAsync();

        return Ok();
    }
}