using Microsoft.EntityFrameworkCore.Migrations;

namespace JokesAPI.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder != null)
            {
                migrationBuilder.CreateTable(
                    name: "JokeItems",
                    columns: table => new
                    {
                        Id = table.Column<long>(nullable: false)
                            .Annotation("Sqlite:Autoincrement", true),
                        Joke = table.Column<string>(maxLength: 255, nullable: false)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_JokeItems", x => x.Id);
                    });
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder != null)
            {
                migrationBuilder.DropTable(
                    name: "JokeItems");
            }
        }
    }
}
