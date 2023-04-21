using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Boolqa.Rapid.App.Data.Migrations
{
    /// <inheritdoc />
    public partial class first : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:object_link_type", "linked")
                .Annotation("Npgsql:Enum:object_type", "object,category,tag,custom")
                .Annotation("Npgsql:Enum:setting_type", "system,user,plugin")
                .Annotation("Npgsql:Enum:shared_mode", "denied,read,write,share")
                .Annotation("Npgsql:Enum:variable_type", "stringboolintegerfloattimedateTimeenum");

            migrationBuilder.CreateTable(
                name: "entity_history",
                columns: table => new
                {
                    entity_history_id = table.Column<Guid>(type: "uuid", nullable: false),
                    entity_id = table.Column<Guid>(type: "uuid", nullable: true),
                    type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    field_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    old_value = table.Column<string>(type: "text", nullable: true),
                    new_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("entity_history_pkey", x => x.entity_history_id);
                });

            migrationBuilder.CreateTable(
                name: "plugin",
                columns: table => new
                {
                    plugin_id = table.Column<Guid>(type: "uuid", nullable: false),
                    plugin_key = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    version = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("plugin_pkey", x => x.plugin_id);
                });

            migrationBuilder.CreateTable(
                name: "group_object",
                columns: table => new
                {
                    object_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_group_object", x => x.object_id);
                });

            migrationBuilder.CreateTable(
                name: "link_object",
                columns: table => new
                {
                    object_id = table.Column<Guid>(type: "uuid", nullable: false),
                    object_from_id = table.Column<Guid>(type: "uuid", nullable: false),
                    object_to_id = table.Column<Guid>(type: "uuid", nullable: false),
                    link_type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_link_object", x => x.object_id);
                });

            migrationBuilder.CreateTable(
                name: "object",
                columns: table => new
                {
                    object_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_object", x => x.object_id);
                });

            migrationBuilder.CreateTable(
                name: "setting_object",
                columns: table => new
                {
                    object_id = table.Column<Guid>(type: "uuid", nullable: false),
                    setting_type = table.Column<int>(type: "integer", nullable: false),
                    variable_type = table.Column<int>(type: "integer", nullable: false),
                    key = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    value = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_setting_object", x => x.object_id);
                    table.ForeignKey(
                        name: "FK_setting_object_object_object_id",
                        column: x => x.object_id,
                        principalTable: "object",
                        principalColumn: "object_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "shared_object",
                columns: table => new
                {
                    object_id = table.Column<Guid>(type: "uuid", nullable: false),
                    target_object_id = table.Column<Guid>(type: "uuid", nullable: true),
                    access_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    mode = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shared_object", x => x.object_id);
                    table.ForeignKey(
                        name: "FK_shared_object_object_object_id",
                        column: x => x.object_id,
                        principalTable: "object",
                        principalColumn: "object_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "shared_object_target_object_id_fkey",
                        column: x => x.target_object_id,
                        principalTable: "object",
                        principalColumn: "object_id");
                });

            migrationBuilder.CreateTable(
                name: "tenant",
                columns: table => new
                {
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    owner_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("tenant_pkey", x => x.tenant_id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: true),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_pkey", x => x.user_id);
                    table.ForeignKey(
                        name: "user_tenant_id_fkey",
                        column: x => x.tenant_id,
                        principalTable: "tenant",
                        principalColumn: "tenant_id");
                });

            migrationBuilder.InsertData(
                table: "user",
                columns: new[] { "user_id", "name", "tenant_id" },
                values: new object[] { new Guid("63d26df9-0e3f-4e00-8187-1a5f7b000001"), "Admin", null });

            migrationBuilder.InsertData(
                table: "object",
                columns: new[] { "object_id", "created_at", "description", "name", "type", "updated_at", "user_id" },
                values: new object[] { new Guid("63d26df9-0e3f-4e00-8187-1a5f7b000003"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Test share", "shared", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("63d26df9-0e3f-4e00-8187-1a5f7b000001") });

            migrationBuilder.InsertData(
                table: "tenant",
                columns: new[] { "tenant_id", "name", "owner_user_id" },
                values: new object[] { new Guid("63d26df9-0e3f-4e00-8187-1a5f7b000002"), "Main", new Guid("63d26df9-0e3f-4e00-8187-1a5f7b000001") });

            migrationBuilder.InsertData(
                table: "shared_object",
                columns: new[] { "object_id", "access_user_id", "mode", "target_object_id" },
                values: new object[] { new Guid("63d26df9-0e3f-4e00-8187-1a5f7b000003"), null, 3, null });

            migrationBuilder.CreateIndex(
                name: "link_object_object_from_id_key",
                table: "link_object",
                column: "object_from_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "link_object_object_to_id_key",
                table: "link_object",
                column: "object_to_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_object_user_id",
                table: "object",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "setting_object_key_key",
                table: "setting_object",
                column: "key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_shared_object_access_user_id",
                table: "shared_object",
                column: "access_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_shared_object_target_object_id",
                table: "shared_object",
                column: "target_object_id");

            migrationBuilder.CreateIndex(
                name: "IX_tenant_owner_user_id",
                table: "tenant",
                column: "owner_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_tenant_id",
                table: "user",
                column: "tenant_id");

            migrationBuilder.AddForeignKey(
                name: "FK_group_object_object_object_id",
                table: "group_object",
                column: "object_id",
                principalTable: "object",
                principalColumn: "object_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_link_object_object_object_id",
                table: "link_object",
                column: "object_id",
                principalTable: "object",
                principalColumn: "object_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "link_object_object_from_id_fkey",
                table: "link_object",
                column: "object_from_id",
                principalTable: "object",
                principalColumn: "object_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "link_object_object_to_id_fkey",
                table: "link_object",
                column: "object_to_id",
                principalTable: "object",
                principalColumn: "object_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "object_user_id_fkey",
                table: "object",
                column: "user_id",
                principalTable: "user",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "shared_object_access_user_id_fkey",
                table: "shared_object",
                column: "access_user_id",
                principalTable: "user",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "tenant_owner_user_id_fkey",
                table: "tenant",
                column: "owner_user_id",
                principalTable: "user",
                principalColumn: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "tenant_owner_user_id_fkey",
                table: "tenant");

            migrationBuilder.DropTable(
                name: "entity_history");

            migrationBuilder.DropTable(
                name: "group_object");

            migrationBuilder.DropTable(
                name: "link_object");

            migrationBuilder.DropTable(
                name: "plugin");

            migrationBuilder.DropTable(
                name: "setting_object");

            migrationBuilder.DropTable(
                name: "shared_object");

            migrationBuilder.DropTable(
                name: "object");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "tenant");
        }
    }
}
