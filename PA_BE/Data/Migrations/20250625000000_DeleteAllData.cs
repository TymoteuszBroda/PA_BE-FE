using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PermAdminAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class DeleteAllData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM EmployeeLicences;");
            migrationBuilder.Sql("DELETE FROM LicenceInstances;");
            migrationBuilder.Sql("DELETE FROM Employees;");
            migrationBuilder.Sql("DELETE FROM Licences;");
            migrationBuilder.Sql("DELETE FROM Users;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Data deletion cannot be reverted
        }
    }
}
