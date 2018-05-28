#pragma warning disable CS1572, CS1573, CS1591
﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using puzzles.Models;

namespace puzzles.Migrations
{
    [DbContext(typeof(PuzzlesDbContext))]
    [Migration("20170509175548_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("puzzles.Models.Puzzle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Puzzles");
                });

            modelBuilder.Entity("puzzles.Models.PuzzleWords", b =>
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

            modelBuilder.Entity("puzzles.Models.PuzzleWords", b =>
                {
                    b.HasOne("puzzles.Models.Puzzle")
                        .WithMany("PuzzleWords")
                        .HasForeignKey("PuzzleId");
                });
        }
    }
}
