using Microsoft.EntityFrameworkCore.Migrations;

namespace TrackerApi.Migrations
{
    public partial class actors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "TvShows",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "LikedIt",
                table: "TvShows",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Actor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ActorTvShow",
                columns: table => new
                {
                    ActorsId = table.Column<int>(type: "INTEGER", nullable: false),
                    TvShowsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActorTvShow", x => new { x.ActorsId, x.TvShowsId });
                    table.ForeignKey(
                        name: "FK_ActorTvShow_Actor_ActorsId",
                        column: x => x.ActorsId,
                        principalTable: "Actor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActorTvShow_TvShows_TvShowsId",
                        column: x => x.TvShowsId,
                        principalTable: "TvShows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActorTvShow_TvShowsId",
                table: "ActorTvShow",
                column: "TvShowsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActorTvShow");

            migrationBuilder.DropTable(
                name: "Actor");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "TvShows");

            migrationBuilder.DropColumn(
                name: "LikedIt",
                table: "TvShows");
        }
    }
}
