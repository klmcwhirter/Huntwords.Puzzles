using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace puzzles.Migrations
{
    public partial class AddTopicsTags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "Puzzles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Topics",
                table: "Puzzles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Puzzles");

            migrationBuilder.DropColumn(
                name: "Topics",
                table: "Puzzles");
        }
    }
}
