using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PermAdminAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeLicencesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeLicences",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    employeeId = table.Column<int>(type: "INTEGER", nullable: false),
                    licenceId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeLicences", x => x.id);
                    table.ForeignKey(
                        name: "FK_EmployeeLicences_Employees_employeeId",
                        column: x => x.employeeId,
                        principalTable: "Employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeLicences_Licences_licenceId",
                        column: x => x.licenceId,
                        principalTable: "Licences",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLicences_employeeId",
                table: "EmployeeLicences",
                column: "employeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLicences_licenceId",
                table: "EmployeeLicences",
                column: "licenceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeLicences");
        }
    }
}
