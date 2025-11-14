using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Filminurk.Data.Migrations
{
    /// <inheritdoc />
    public partial class filesToDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FavouriteListID",
                table: "Movies",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FavouriteLists",
                columns: table => new
                {
                    FavouriteListID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ListBelongsToUser = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsMovieOrActor = table.Column<bool>(type: "bit", nullable: false),
                    ListName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ListDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPrviate = table.Column<bool>(type: "bit", nullable: false),
                    ListCreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ListModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ListDeletedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsReported = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavouriteLists", x => x.FavouriteListID);
                });

            migrationBuilder.CreateTable(
                name: "FilesToDatabase",
                columns: table => new
                {
                    ImageID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageData = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ListID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilesToDatabase", x => x.ImageID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movies_FavouriteListID",
                table: "Movies",
                column: "FavouriteListID");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_FavouriteLists_FavouriteListID",
                table: "Movies",
                column: "FavouriteListID",
                principalTable: "FavouriteLists",
                principalColumn: "FavouriteListID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_FavouriteLists_FavouriteListID",
                table: "Movies");

            migrationBuilder.DropTable(
                name: "FavouriteLists");

            migrationBuilder.DropTable(
                name: "FilesToDatabase");

            migrationBuilder.DropIndex(
                name: "IX_Movies_FavouriteListID",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "FavouriteListID",
                table: "Movies");
        }
    }
}
