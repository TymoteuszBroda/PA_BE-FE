using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PermAdminAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixDateDefaults : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Normalize unrealistic existing values
            migrationBuilder.Sql(@"
                UPDATE EmployeeLicences
                SET AssignedOn = CURRENT_TIMESTAMP
                WHERE AssignedOn IN ('1970-01-01 00:00:00', '0001-01-01 00:00:00');
            ");

            migrationBuilder.Sql(@"
                UPDATE Licences
                SET ValidTo = datetime('now','+1 year')
                WHERE ValidTo IN ('0001-01-01 00:00:00');
            ");

            migrationBuilder.Sql(@"
                UPDATE LicenceInstances
                SET ValidTo = datetime('now','+1 year')
                WHERE ValidTo IN ('0001-01-01 00:00:00');
            ");

            // Recreate Licences table with dynamic default
            migrationBuilder.Sql(@"
                CREATE TABLE Licences_temp (
                    id INTEGER NOT NULL CONSTRAINT PK_Licences PRIMARY KEY AUTOINCREMENT,
                    ApplicationName TEXT NOT NULL,
                    Quantity INTEGER NOT NULL,
                    ValidTo TEXT NOT NULL DEFAULT (datetime('now','+1 year'))
                );
                INSERT INTO Licences_temp (id, ApplicationName, Quantity, ValidTo)
                SELECT id, ApplicationName, Quantity, ValidTo FROM Licences;
                DROP TABLE Licences;
                ALTER TABLE Licences_temp RENAME TO Licences;
            ");

            // Recreate LicenceInstances table with dynamic default
            migrationBuilder.Sql(@"
                CREATE TABLE LicenceInstances_temp (
                    Id INTEGER NOT NULL CONSTRAINT PK_LicenceInstances PRIMARY KEY AUTOINCREMENT,
                    LicenceId INTEGER NOT NULL,
                    ValidTo TEXT NOT NULL DEFAULT (datetime('now','+1 year')),
                    FOREIGN KEY (LicenceId) REFERENCES Licences(id) ON DELETE CASCADE
                );
                INSERT INTO LicenceInstances_temp (Id, LicenceId, ValidTo)
                SELECT Id, LicenceId, ValidTo FROM LicenceInstances;
                DROP TABLE LicenceInstances;
                ALTER TABLE LicenceInstances_temp RENAME TO LicenceInstances;
            ");

            migrationBuilder.CreateIndex(
                name: "IX_LicenceInstances_LicenceId",
                table: "LicenceInstances",
                column: "LicenceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE TABLE Licences_temp (
                    id INTEGER NOT NULL CONSTRAINT PK_Licences PRIMARY KEY AUTOINCREMENT,
                    ApplicationName TEXT NOT NULL,
                    Quantity INTEGER NOT NULL,
                    ValidTo TEXT NOT NULL DEFAULT '2025-12-31 00:00:00'
                );
                INSERT INTO Licences_temp (id, ApplicationName, Quantity, ValidTo)
                SELECT id, ApplicationName, Quantity, ValidTo FROM Licences;
                DROP TABLE Licences;
                ALTER TABLE Licences_temp RENAME TO Licences;
            ");

            migrationBuilder.Sql(@"
                CREATE TABLE LicenceInstances_temp (
                    Id INTEGER NOT NULL CONSTRAINT PK_LicenceInstances PRIMARY KEY AUTOINCREMENT,
                    LicenceId INTEGER NOT NULL,
                    ValidTo TEXT NOT NULL,
                    FOREIGN KEY (LicenceId) REFERENCES Licences(id) ON DELETE CASCADE
                );
                INSERT INTO LicenceInstances_temp (Id, LicenceId, ValidTo)
                SELECT Id, LicenceId, ValidTo FROM LicenceInstances;
                DROP TABLE LicenceInstances;
                ALTER TABLE LicenceInstances_temp RENAME TO LicenceInstances;
            ");

            migrationBuilder.CreateIndex(
                name: "IX_LicenceInstances_LicenceId",
                table: "LicenceInstances",
                column: "LicenceId");
        }
    }
}
