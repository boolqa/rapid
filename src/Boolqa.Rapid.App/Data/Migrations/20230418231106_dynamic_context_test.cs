using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Boolqa.Rapid.App.Data.Migrations
{
    /// <inheritdoc />
    public partial class dynamic_context_test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "hello_world",
                columns: table => new
                {
                    object_id = table.Column<Guid>(type: "uuid", nullable: false),
                    log_text = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hello_world", x => x.object_id);
                    table.ForeignKey(
                        name: "FK_hello_world_object_object_id",
                        column: x => x.object_id,
                        principalTable: "object",
                        principalColumn: "object_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "object",
                keyColumn: "object_id",
                keyValue: new Guid("63d26df9-0e3f-4e00-8187-1a5f7b000003"),
                column: "type",
                value: "shared_object");

            migrationBuilder.UpdateData(
                table: "shared_object",
                keyColumn: "object_id",
                keyValue: new Guid("63d26df9-0e3f-4e00-8187-1a5f7b000003"),
                column: "mode",
                value: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "hello_world");

            migrationBuilder.UpdateData(
                table: "object",
                keyColumn: "object_id",
                keyValue: new Guid("63d26df9-0e3f-4e00-8187-1a5f7b000003"),
                column: "type",
                value: "shared");

            migrationBuilder.UpdateData(
                table: "shared_object",
                keyColumn: "object_id",
                keyValue: new Guid("63d26df9-0e3f-4e00-8187-1a5f7b000003"),
                column: "mode",
                value: 3);
        }
    }
}
