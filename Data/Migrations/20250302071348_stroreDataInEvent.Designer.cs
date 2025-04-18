﻿// <auto-generated />
using System;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250302071348_stroreDataInEvent")]
    partial class stroreDataInEvent
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Data.Models.journal_event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Exception")
                        .HasColumnType("text");

                    b.Property<string>("ExceptionMessage")
                        .HasColumnType("text");

                    b.Property<string>("ExceptionStackTrace")
                        .HasColumnType("text");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RequestBody")
                        .HasColumnType("text");

                    b.Property<string>("RequestQuery")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("journal_events");
                });

            modelBuilder.Entity("Data.Models.journal_message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("EventId")
                        .HasColumnType("integer")
                        .HasColumnName("event_id");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.HasKey("Id");

                    b.HasIndex("EventId")
                        .IsUnique();

                    b.ToTable("journal_messages", (string)null);
                });

            modelBuilder.Entity("Data.tree_node", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<int?>("ParentNodeId")
                        .HasColumnType("integer")
                        .HasColumnName("parent_node_id");

                    b.Property<string>("TreeName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ParentNodeId");

                    b.ToTable("tree_nodes", (string)null);
                });

            modelBuilder.Entity("Data.Models.journal_message", b =>
                {
                    b.HasOne("Data.Models.journal_event", "JournalEvent")
                        .WithOne("Message")
                        .HasForeignKey("Data.Models.journal_message", "EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("JournalEvent");
                });

            modelBuilder.Entity("Data.tree_node", b =>
                {
                    b.HasOne("Data.tree_node", "ParentNode")
                        .WithMany("Children")
                        .HasForeignKey("ParentNodeId");

                    b.Navigation("ParentNode");
                });

            modelBuilder.Entity("Data.Models.journal_event", b =>
                {
                    b.Navigation("Message")
                        .IsRequired();
                });

            modelBuilder.Entity("Data.tree_node", b =>
                {
                    b.Navigation("Children");
                });
#pragma warning restore 612, 618
        }
    }
}
