using Medicines.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Medicines.Controllers;

[ApiController]
[Route("[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly MedicinesContext _context;

    public DoctorsController(MedicinesContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_context.Doctors.FromSqlRaw(@"select * from Doctors d 
                                                 join Hospitals h on h.Id = d.HospitalId"));
    }

    [HttpGet("{id:guid}")]
    public IActionResult Get(Guid id)
    {
        return Ok(_context.Doctors.FromSqlRaw(@"select * from Doctors d 
                                                 join Hospitals h on h.Id = d.HospitalId 
                                                 where d.Id = {0}", id));
    }

    [HttpPost]
    public async Task<IActionResult> Create(Doctor doctor)
    {
        var id = Guid.NewGuid();
        doctor.Id = id;
        var hospital = _context.Hospitals.FromSqlRaw("select * from Hospitals where Id = {0}", doctor.HospitalId);
        if (hospital.FirstOrDefault() is null)
            return BadRequest();

        _context.Doctors.FromSqlRaw(@"insert into Doctors(Name, Id, License, HospitalId) 
                                                    values({0}, {1}, {2}, {3})",
            doctor.Name, doctor.Id, doctor.License, doctor.HospitalId);

        await _context.SaveChangesAsync();

        return Ok(id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, Doctor doctor)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        var model = _context.Doctors.FromSqlRaw(@"select * from Doctors where Id = {0}", id);
        if (model.FirstOrDefault() is null)
            return BadRequest();

        doctor.Id = id;

        var hospital = _context.Hospitals.FromSqlRaw("select * from Hospitals where Id = {0}", doctor.HospitalId);
        if (hospital.FirstOrDefault() is null)
            return BadRequest();

        _context.Doctors.FromSqlRaw("update Doctors set License = {0}, Name = {1}, HospitalId = {2} where Id = {3}",
            doctor.License, doctor.Name, doctor.HospitalId, doctor.Id);
        await _context.SaveChangesAsync();
        await _context.Database.CommitTransactionAsync();

        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var model = _context.Doctors.FromSqlRaw(@"select * from Doctors where Id = {0}", id);
        if (model.FirstOrDefault() is null)
            return BadRequest();

        _context.Doctors.FromSqlRaw("delete from Doctors where Id = {0}", id);
        await _context.SaveChangesAsync();

        return Ok();
    }
}