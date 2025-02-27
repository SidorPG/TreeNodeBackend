using Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<journal_message> JournalMessages { get; set; }
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
