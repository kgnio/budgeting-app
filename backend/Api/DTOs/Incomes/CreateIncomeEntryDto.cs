namespace Api.DTOs.Incomes
{
    public class CreateIncomeEntryDto
    {
        public decimal Amount { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string? Note { get; set; }
    }
}
