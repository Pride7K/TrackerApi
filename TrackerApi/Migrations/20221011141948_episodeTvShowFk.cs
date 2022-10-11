using Microsoft.EntityFrameworkCore.Migrations;

namespace TrackerApi.Migrations
{
    public partial class episodeTvShowFk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Episodes_TvShows_TvShowId",
                table: "Episodes");

            migrationBuilder.AlterColumn<int>(
                name: "TvShowId",
                table: "Episodes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Episodes_TvShows_TvShowId",
                table: "Episodes",
                column: "TvShowId",
                principalTable: "TvShows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Episodes_TvShows_TvShowId",
                table: "Episodes");

            migrationBuilder.AlterColumn<int>(
                name: "TvShowId",
                table: "Episodes",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Episodes_TvShows_TvShowId",
                table: "Episodes",
                column: "TvShowId",
                principalTable: "TvShows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
