using Medicines.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Medicines.Controllers;

[ApiController]
[Route("[controller]")]
public class RecipesController : ControllerBase
{
    private readonly MedicinesContext _context;

    public RecipesController(MedicinesContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_context.Recipes.FromSqlRaw(@"select * from Recipes r 
                                                 join Doctors d on d.Id = r.DoctorId
                                                 join RecipesMedicines rm on r.Id = rm.RecipeId
                                                 join Medicines m on m.Id = rm.MedicineId"));
    }

    [HttpGet("{id:guid}")]
    public IActionResult Get(Guid id)
    {
        return Ok(_context.Doctors.FromSqlRaw(@"select * from Recipes r 
                                                 join Doctors d on d.Id = r.DoctorId
                                                 join RecipesMedicines rm on r.Id = rm.RecipeId
                                                 join Medicines m on m.Id = rm.MedicineId
                                                 where r.Id = {0}", id));
    }

    [HttpPost]
    public async Task<IActionResult> Create(Recipe recipe)
    {
        var sql = recipe.RecipesMedicines.Aggregate(
            @"declare @medicineTableType MedicineTableType, @id uniqueidentifier",
            (current, recipesMedicine) => current +
                                          $"\ninsert into @medicineTableType(MedicineId, Count) values({recipesMedicine.MedicineId}, {recipesMedicine.Count})");
        sql +=
            $@"
                exec insertRecipe {recipe.DoctorId}, @medicineTableType, {recipe.DateOfGiving}, {recipe.ValidityPeriod}, @id output
                select @id";

        var id = _context.Recipes.FromSqlRaw(sql);

        await _context.SaveChangesAsync();

        return Ok(id.FirstOrDefault());
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var model = _context.Recipes.FromSqlRaw(@"select * from Recipes where Id = {0}", id);
        if (model.FirstOrDefault() is null)
            return BadRequest();

        _context.Recipes.FromSqlRaw("delete from Recipes where Id = {0}", id);
        await _context.SaveChangesAsync();

        return Ok();
    }
}