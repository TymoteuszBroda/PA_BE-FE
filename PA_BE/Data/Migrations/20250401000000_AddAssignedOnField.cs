using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PermAdminAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAssignedOnField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // SQLite cannot add a column with a non constant default value. To
            // keep the CURRENT_TIMESTAMP default we recreate the table with the
            // new column and copy existing data.
            migrationBuilder.Sql(@"
                CREATE TABLE EmployeeLicences_temp (
                    id INTEGER NOT NULL CONSTRAINT PK_EmployeeLicences PRIMARY KEY AUTOINCREMENT,
                    employeeId INTEGER NOT NULL,
                    licenceId INTEGER NOT NULL,
                    AssignedOn TEXT NOT NULL DEFAULT (CURRENT_TIMESTAMP),
                    FOREIGN KEY (employeeId) REFERENCES Employees(id) ON DELETE CASCADE,
                    FOREIGN KEY (licenceId) REFERENCES Licences(id) ON DELETE CASCADE
                );
                INSERT INTO EmployeeLicences_temp (id, employeeId, licenceId, AssignedOn)
                SELECT id, employeeId, licenceId, CURRENT_TIMESTAMP FROM EmployeeLicences;
                DROP TABLE EmployeeLicences;
                ALTER TABLE EmployeeLicences_temp RENAME TO EmployeeLicences;
            ");

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
            // Recreate table without the AssignedOn column and copy back data
            migrationBuilder.Sql(@"
                CREATE TABLE EmployeeLicences_temp (
                    id INTEGER NOT NULL CONSTRAINT PK_EmployeeLicences PRIMARY KEY AUTOINCREMENT,
                    employeeId INTEGER NOT NULL,
                    licenceId INTEGER NOT NULL,
                    FOREIGN KEY (employeeId) REFERENCES Employees(id) ON DELETE CASCADE,
                    FOREIGN KEY (licenceId) REFERENCES Licences(id) ON DELETE CASCADE
                );
                INSERT INTO EmployeeLicences_temp (id, employeeId, licenceId)
                SELECT id, employeeId, licenceId FROM EmployeeLicences;
                DROP TABLE EmployeeLicences;
                ALTER TABLE EmployeeLicences_temp RENAME TO EmployeeLicences;
            ");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLicences_employeeId",
                table: "EmployeeLicences",
                column: "employeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLicences_licenceId",
                table: "EmployeeLicences",
                column: "licenceId");
        }
    }
}
