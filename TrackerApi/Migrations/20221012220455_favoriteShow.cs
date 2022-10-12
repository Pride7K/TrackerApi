using Microsoft.EntityFrameworkCore.Migrations;

namespace TrackerApi.Migrations
{
    public partial class favoriteShow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserTvShowFavorite",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    TvShowsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTvShowFavorite", x => new { x.UserId, x.TvShowsId });
                    table.ForeignKey(
                        name: "FK_UserTvShowFavorite_TvShows_TvShowsId",
                        column: x => x.TvShowsId,
                        principalTable: "TvShows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTvShowFavorite_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTvShowFavorite_TvShowsId",
                table: "UserTvShowFavorite",
                column: "TvShowsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTvShowFavorite");
        }
    }
}
