using Microsoft.EntityFrameworkCore.Migrations;

namespace Movies.Infrastructure.Migrations
{
    public partial class repliesupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoritedMovies_Movies_MovieId",
                table: "FavoritedMovies");

            migrationBuilder.RenameColumn(
                name: "MovieId",
                table: "FavoritedMovies",
                newName: "MovieID");

            migrationBuilder.RenameIndex(
                name: "IX_FavoritedMovies_MovieId",
                table: "FavoritedMovies",
                newName: "IX_FavoritedMovies_MovieID");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Replies",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MovieID",
                table: "FavoritedMovies",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FavoritedMovies_Movies_MovieID",
                table: "FavoritedMovies",
                column: "MovieID",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoritedMovies_Movies_MovieID",
                table: "FavoritedMovies");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "Replies");

            migrationBuilder.RenameColumn(
                name: "MovieID",
                table: "FavoritedMovies",
                newName: "MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_FavoritedMovies_MovieID",
                table: "FavoritedMovies",
                newName: "IX_FavoritedMovies_MovieId");

            migrationBuilder.AlterColumn<int>(
                name: "MovieId",
                table: "FavoritedMovies",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_FavoritedMovies_Movies_MovieId",
                table: "FavoritedMovies",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
