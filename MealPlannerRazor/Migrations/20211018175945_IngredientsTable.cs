using Microsoft.EntityFrameworkCore.Migrations;

namespace MealPlannerRazor.Migrations
{
    public partial class IngredientsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "RecipeModel",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "IngredientModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    MeasureType = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngredientModel", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IngredientModel");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "RecipeModel");
        }
    }
}
