using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Filminurk.Data.Migrations
{
    /// <inheritdoc />
    public partial class shittyField2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Actors_ActorID",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_ActorID",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "ActorID",
                table: "Movies");

            migrationBuilder.AddColumn<string>(
                name: "MoviesActedFor",
                table: "Actors",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MoviesActedFor",
                table: "Actors");

            migrationBuilder.AddColumn<Guid>(
                name: "ActorID",
                table: "Movies",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Movies_ActorID",
                table: "Movies",
                column: "ActorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Actors_ActorID",
                table: "Movies",
                column: "ActorID",
                principalTable: "Actors",
                principalColumn: "ActorID");
        }
    }
}
