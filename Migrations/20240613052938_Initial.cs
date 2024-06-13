using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TasksWebApi.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "category",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    weight = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "task",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task", x => x.id);
                    table.ForeignKey(
                        name: "FK_task_category_category_id",
                        column: x => x.category_id,
                        principalTable: "category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "category",
                columns: new[] { "id", "created_at", "description", "name", "updated_at", "weight" },
                values: new object[,]
                {
                    { new Guid("e9d2de54-d048-42fd-8715-251875766097"), new DateTime(2024, 6, 13, 5, 29, 38, 120, DateTimeKind.Utc).AddTicks(710), "", "Actividades Pendientes", new DateTime(2024, 6, 13, 5, 29, 38, 120, DateTimeKind.Utc).AddTicks(710), 20 },
                    { new Guid("ea5fdb05-f70e-47e0-91ee-292f1baf5eae"), new DateTime(2024, 6, 13, 5, 29, 38, 120, DateTimeKind.Utc).AddTicks(740), "", "Actividades Personales", new DateTime(2024, 6, 13, 5, 29, 38, 120, DateTimeKind.Utc).AddTicks(740), 50 }
                });

            migrationBuilder.InsertData(
                table: "task",
                columns: new[] { "id", "category_id", "created_at", "description", "name", "priority", "updated_at" },
                values: new object[,]
                {
                    { new Guid("3c3fd5ab-2202-4dea-82ea-1ef1dc7c9b20"), new Guid("e9d2de54-d048-42fd-8715-251875766097"), new DateTime(2024, 6, 13, 5, 29, 38, 120, DateTimeKind.Utc).AddTicks(6550), null, "Pago de servicios públicos", 1, new DateTime(2024, 6, 13, 5, 29, 38, 120, DateTimeKind.Utc).AddTicks(6550) },
                    { new Guid("94cbd857-fd05-496a-b025-673815fdf7b8"), new Guid("ea5fdb05-f70e-47e0-91ee-292f1baf5eae"), new DateTime(2024, 6, 13, 5, 29, 38, 120, DateTimeKind.Utc).AddTicks(6560), null, "Terminar película en Netflix", 0, new DateTime(2024, 6, 13, 5, 29, 38, 120, DateTimeKind.Utc).AddTicks(6560) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_task_category_id",
                table: "task",
                column: "category_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "task");

            migrationBuilder.DropTable(
                name: "category");
        }
    }
}
