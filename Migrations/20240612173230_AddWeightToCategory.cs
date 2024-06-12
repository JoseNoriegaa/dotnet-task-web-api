using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ef_practie.Migrations
{
    /// <inheritdoc />
    public partial class AddWeightToCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "weight",
                table: "category",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "weight",
                table: "category");
        }
    }
}
