using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceScalingDb.Migrations
{
    /// <inheritdoc />
    public partial class uniquecombinationforapplicationtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Applications_ApplicationName_ProjectID",
                table: "Applications",
                columns: new[] { "ApplicationName", "ProjectID" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Applications_ApplicationName_ProjectID",
                table: "Applications");
        }
    }
}
