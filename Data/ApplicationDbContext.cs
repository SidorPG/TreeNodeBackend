using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<journal_message> JournalMessages { get; set; }
    public DbSet<journal_event> JournalEvents { get; set; }
    public DbSet<tree_node> TreeNodes { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new JournalMessageConfiguration());
        modelBuilder.ApplyConfiguration(new TreeNodeConfiguration());
    }
}
