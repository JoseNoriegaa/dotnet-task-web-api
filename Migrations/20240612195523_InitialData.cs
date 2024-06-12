using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ef_practie.Migrations
{
    /// <inheritdoc />
    public partial class InitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "task",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.InsertData(
                table: "category",
                columns: new[] { "id", "created_at", "description", "name", "updated_at", "weight" },
                values: new object[,]
                {
                    { new Guid("e9d2de54-d048-42fd-8715-251875766097"), new DateTime(2024, 6, 12, 19, 55, 22, 958, DateTimeKind.Utc), "", "Actividades Pendientes", new DateTime(2024, 6, 12, 19, 55, 22, 958, DateTimeKind.Utc), 20 },
                    { new Guid("ea5fdb05-f70e-47e0-91ee-292f1baf5eae"), new DateTime(2024, 6, 12, 19, 55, 22, 958, DateTimeKind.Utc).AddTicks(10), "", "Actividades Personales", new DateTime(2024, 6, 12, 19, 55, 22, 958, DateTimeKind.Utc).AddTicks(10), 50 }
                });

            migrationBuilder.InsertData(
                table: "task",
                columns: new[] { "id", "category_id", "created_at", "description", "name", "priority", "updated_at" },
                values: new object[,]
                {
                    { new Guid("3c3fd5ab-2202-4dea-82ea-1ef1dc7c9b20"), new Guid("e9d2de54-d048-42fd-8715-251875766097"), new DateTime(2024, 6, 12, 19, 55, 22, 958, DateTimeKind.Utc).AddTicks(1990), "", "Pago de servicios públicos", 1, new DateTime(2024, 6, 12, 19, 55, 22, 958, DateTimeKind.Utc).AddTicks(1990) },
                    { new Guid("94cbd857-fd05-496a-b025-673815fdf7b8"), new Guid("ea5fdb05-f70e-47e0-91ee-292f1baf5eae"), new DateTime(2024, 6, 12, 19, 55, 22, 958, DateTimeKind.Utc).AddTicks(2000), "", "Terminar película en Netflix", 0, new DateTime(2024, 6, 12, 19, 55, 22, 958, DateTimeKind.Utc).AddTicks(2000) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "task",
                keyColumn: "id",
                keyValue: new Guid("3c3fd5ab-2202-4dea-82ea-1ef1dc7c9b20"));

            migrationBuilder.DeleteData(
                table: "task",
                keyColumn: "id",
                keyValue: new Guid("94cbd857-fd05-496a-b025-673815fdf7b8"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("e9d2de54-d048-42fd-8715-251875766097"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("ea5fdb05-f70e-47e0-91ee-292f1baf5eae"));

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "task",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
