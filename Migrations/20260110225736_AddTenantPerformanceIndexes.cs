using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatientMenu.Migrations
{
    /// <inheritdoc />
    public partial class AddTenantPerformanceIndexes : Migration
    {
        /// <inheritdoc />
              protected override void Up(MigrationBuilder migrationBuilder)
        {
            // MenuItems: Tenant-based menu filtering
            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_Tenant_Restrictions",
                table: "MenuItems",
                columns: new[] { "TenantId", "Id" })
                .Annotation("SqlServer:Include", new[]
                {
                "IsGlutenFree",
                "IsSugarFree",
                "IsHeartHealthy",
                "Name",
                "Category"
                });

            // Patients: Tenant + Patient lookup
            migrationBuilder.CreateIndex(
                name: "IX_Patients_Tenant_Patient",
                table: "Patients",
                columns: new[] { "TenantId", "Id" })
                .Annotation("SqlServer:Include", new[]
                {
                "DietaryRestrictionCode"
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MenuItems_Tenant_Restrictions",
                table: "MenuItems");

            migrationBuilder.DropIndex(
                name: "IX_Patients_Tenant_Patient",
                table: "Patients");
        }
    }
}
