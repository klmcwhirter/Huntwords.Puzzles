using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using puzzles.Models;

namespace puzzles.Migrations
{
    [DbContext(typeof(PuzzlesDbContext))]
    partial class PuzzlesDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("puzzles.Models.Puzzle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<string>("Tags");

                    b.Property<string>("Topics");

                    b.HasKey("Id");

                    b.ToTable("Puzzles");
                });

            modelBuilder.Entity("puzzles.Models.PuzzleWord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("PuzzleId");

                    b.Property<string>("Word");

                    b.HasKey("Id");

                    b.HasIndex("PuzzleId");

                    b.ToTable("PuzzleWords");
                });

            modelBuilder.Entity("puzzles.Models.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("puzzles.Models.Topic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Topics");
                });

            modelBuilder.Entity("puzzles.Models.PuzzleWord", b =>
                {
                    b.HasOne("puzzles.Models.Puzzle")
                        .WithMany("PuzzleWords")
                        .HasForeignKey("PuzzleId");
                });
        }
    }
}
