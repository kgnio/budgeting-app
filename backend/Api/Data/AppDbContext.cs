using Api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<IncomeEntry> IncomeEntries => Set<IncomeEntry>();
        public DbSet<ExpenseEntry> ExpenseEntries => Set<ExpenseEntry>();
        public DbSet<ExpenseCategory> ExpenseCategories => Set<ExpenseCategory>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ExpenseCategory>().HasIndex(x => x.Name).IsUnique();

            builder.Entity<IncomeEntry>().Property(x => x.Amount).HasColumnType("numeric(18,2)");

            builder.Entity<ExpenseEntry>().Property(x => x.Amount).HasColumnType("numeric(18,2)");

            builder
                .Entity<IncomeEntry>()
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Entity<ExpenseEntry>()
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Entity<ExpenseEntry>()
                .HasOne(x => x.Category)
                .WithMany(x => x.ExpenseEntries)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
