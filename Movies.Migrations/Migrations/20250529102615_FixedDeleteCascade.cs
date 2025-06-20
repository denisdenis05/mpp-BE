﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Movies.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class FixedDeleteCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Endorsements_Movies_MovieId",
                table: "Endorsements");

            migrationBuilder.AlterColumn<int>(
                name: "MovieId",
                table: "Endorsements",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Endorsements_Movies_MovieId",
                table: "Endorsements",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Endorsements_Movies_MovieId",
                table: "Endorsements");

            migrationBuilder.AlterColumn<int>(
                name: "MovieId",
                table: "Endorsements",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Endorsements_Movies_MovieId",
                table: "Endorsements",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id");
        }
    }
}
