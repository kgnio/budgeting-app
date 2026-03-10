using Api.Data;
using Api.DTOs.Incomes;
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
    public class IncomesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public IncomesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<IncomeEntryDto>>> GetMine()
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var incomes = await _context
                .IncomeEntries.Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Month)
                .ThenByDescending(x => x.CreatedAt)
                .Select(x => new IncomeEntryDto
                {
                    Id = x.Id,
                    Amount = x.Amount,
                    Month = x.Month,
                    Year = x.Year,
                    Note = x.Note,
                    CreatedAt = x.CreatedAt,
                })
                .ToListAsync();

            return Ok(incomes);
        }

        [HttpPost]
        public async Task<ActionResult<IncomeEntryDto>> Create(CreateIncomeEntryDto dto)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var income = new IncomeEntry
            {
                UserId = userId,
                Amount = dto.Amount,
                Month = dto.Month,
                Year = dto.Year,
                Note = dto.Note,
            };

            _context.IncomeEntries.Add(income);
            await _context.SaveChangesAsync();

            return Ok(
                new IncomeEntryDto
                {
                    Id = income.Id,
                    Amount = income.Amount,
                    Month = income.Month,
                    Year = income.Year,
                    Note = income.Note,
                    CreatedAt = income.CreatedAt,
                }
            );
        }
    }
}
