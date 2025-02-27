namespace Data.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class TreeNodeConfiguration : IEntityTypeConfiguration<tree_node>
{
    public void Configure(EntityTypeBuilder<tree_node> builder)
    {
        builder.ToTable("tree_nodes");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.ParentNodeId).HasColumnName("parent_node_id");

        builder.HasMany(x => x.Children)
            .WithOne(x => x.ParentNode)
            .HasForeignKey(x => x.ParentNodeId).IsRequired(false);
    }
}
