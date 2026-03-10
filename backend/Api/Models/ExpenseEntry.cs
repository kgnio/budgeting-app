namespace Api.Models
{
    public enum ExpenseType
    {
        OneTime = 1,
        RecurringMonthly = 2,
    }

    public class ExpenseEntry
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;

        public int CategoryId { get; set; }
        public ExpenseCategory Category { get; set; } = null!;

        public string Title { get; set; } = string.Empty;
        public decimal Amount { get; set; }

        public ExpenseType Type { get; set; }

        public DateTime? ExpenseDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string? Note { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
