using Api.Data;
using Api.DTOs.Expenses;
using Api.Helpers;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ExpensesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ExpensesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseEntryDto>>> GetMine()
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var expenses = await _context
                .ExpenseEntries.Include(x => x.Category)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new ExpenseEntryDto
                {
                    Id = x.Id,
                    CategoryId = x.CategoryId,
                    CategoryName = x.Category.Name,
                    Title = x.Title,
                    Amount = x.Amount,
                    Type = x.Type,
                    ExpenseDate = x.ExpenseDate,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Note = x.Note,
                    CreatedAt = x.CreatedAt,
                })
                .ToListAsync();

            return Ok(expenses);
        }

        [HttpPost]
        public async Task<ActionResult<ExpenseEntryDto>> Create(CreateExpenseEntryDto dto)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var category = await _context.ExpenseCategories.FirstOrDefaultAsync(x =>
                x.Id == dto.CategoryId
            );

            if (category == null)
            {
                return BadRequest(new { message = "Category not found." });
            }

            if (dto.Type == ExpenseType.OneTime && dto.ExpenseDate == null)
            {
                return BadRequest(
                    new { message = "ExpenseDate is required for one-time expenses." }
                );
            }

            if (dto.Type == ExpenseType.RecurringMonthly && dto.StartDate == null)
            {
                return BadRequest(
                    new { message = "StartDate is required for recurring monthly expenses." }
                );
            }

            var expense = new ExpenseEntry
            {
                UserId = userId,
                CategoryId = dto.CategoryId,
                Title = dto.Title,
                Amount = dto.Amount,
                Type = dto.Type,
                ExpenseDate = dto.ExpenseDate,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Note = dto.Note,
            };

            _context.ExpenseEntries.Add(expense);
            await _context.SaveChangesAsync();

            return Ok(
                new ExpenseEntryDto
                {
                    Id = expense.Id,
                    CategoryId = expense.CategoryId,
                    CategoryName = category.Name,
                    Title = expense.Title,
                    Amount = expense.Amount,
                    Type = expense.Type,
                    ExpenseDate = expense.ExpenseDate,
                    StartDate = expense.StartDate,
                    EndDate = expense.EndDate,
                    Note = expense.Note,
                    CreatedAt = expense.CreatedAt,
                }
            );
        }
    }
}
