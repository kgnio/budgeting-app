namespace Api.DTOs.Incomes
{
    public class IncomeEntryDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string? Note { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
