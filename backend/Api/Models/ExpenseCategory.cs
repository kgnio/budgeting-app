namespace Api.Models
{
    public class ExpenseCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<ExpenseEntry> ExpenseEntries { get; set; } = new List<ExpenseEntry>();
    }
}
