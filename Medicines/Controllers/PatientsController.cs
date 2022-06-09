using Medicines.Models;
using Microsoft.AspNetCore.Mvc;

namespace Medicines.Controllers;

[ApiController]
[Route("[controller]")]
public class PatientsController : ControllerBase
{
    private readonly MedicinesContext _context;

    public PatientsController(MedicinesContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_context.Patients.ToList());
    }

    [HttpGet("{id:guid}")]
    public IActionResult Get(Guid id)
    {
        return Ok(_context.Patients.SingleOrDefault(p => p.Id == id));
    }

    [HttpPost]
    public async Task<IActionResult> Create(Patient patient)
    {
        var id = Guid.NewGuid();
        patient.Id = id;

        _context.Patients.Add(patient);

        await _context.SaveChangesAsync();

        return Ok(id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, Patient patient)
    {
        var model = _context.Patients.SingleOrDefault(p => p.Id == id);
        if (model is null)
            return BadRequest();

        patient.Id = id;

        _context.Patients.Update(patient);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var model = _context.Patients.SingleOrDefault(p => p.Id == id);
        if (model is null)
            return BadRequest();

        _context.Patients.Remove(model);
        await _context.SaveChangesAsync();

        return Ok();
    }
}