using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PermAdminAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class licenseInstance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ValidTo",
                table: "Licences",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2025, 12, 31));

            migrationBuilder.CreateTable(
                name: "LicenceInstances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LicenceId = table.Column<int>(type: "INTEGER", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenceInstances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LicenceInstances_Licences_LicenceId",
                        column: x => x.LicenceId,
                        principalTable: "Licences",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LicenceInstances_LicenceId",
                table: "LicenceInstances",
                column: "LicenceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LicenceInstances");

            migrationBuilder.DropColumn(
                name: "ValidTo",
                table: "Licences");
        }
    }
}
