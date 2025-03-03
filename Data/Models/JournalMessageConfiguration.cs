﻿namespace Data.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class JournalMessageConfiguration : IEntityTypeConfiguration<journal_message>
{
    public void Configure(EntityTypeBuilder<journal_message> builder)
    {
        builder.ToTable("journal_messages");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.EventId).HasColumnName("event_id");
        builder.Property(x => x.Type).HasColumnName("type");

        builder.HasOne(x => x.JournalEvent)
            .WithOne(x => x.Message)
            .HasForeignKey<journal_message>(x => x.EventId);
    }
}
