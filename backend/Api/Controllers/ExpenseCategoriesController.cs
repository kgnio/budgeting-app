using Api.Data;
using Api.DTOs.Categories;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ExpenseCategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ExpenseCategoriesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseCategoryDto>>> GetAll()
        {
            var categories = await _context
                .ExpenseCategories.OrderBy(x => x.Name)
                .Select(x => new ExpenseCategoryDto { Id = x.Id, Name = x.Name })
                .ToListAsync();

            return Ok(categories);
        }

        [HttpPost]
        public async Task<ActionResult<ExpenseCategoryDto>> Create(CreateExpenseCategoryDto dto)
        {
            var exists = await _context.ExpenseCategories.AnyAsync(x =>
                x.Name.ToLower() == dto.Name.ToLower()
            );

            if (exists)
            {
                return BadRequest(new { message = "Category already exists." });
            }

            var category = new ExpenseCategory { Name = dto.Name.Trim() };

            _context.ExpenseCategories.Add(category);
            await _context.SaveChangesAsync();

            return Ok(new ExpenseCategoryDto { Id = category.Id, Name = category.Name });
        }
    }
}
