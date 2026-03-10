using Api.Models;

namespace Api.DTOs.Expenses
{
    public class CreateExpenseEntryDto
    {
        public int CategoryId { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public ExpenseType Type { get; set; }
        public DateTime? ExpenseDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Note { get; set; }
    }
}
