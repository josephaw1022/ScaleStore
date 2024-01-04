using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceScalingDb.Migrations
{
    /// <inheritdoc />
    public partial class newfkandindexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScalingConfigurations_Applications_ApplicationID",
                table: "ScalingConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_ScalingConfigurations_Environments_EnvironmentID",
                table: "ScalingConfigurations");

            migrationBuilder.AddForeignKey(
                name: "FK_ScalingConfigurations_Applications_ApplicationID",
                table: "ScalingConfigurations",
                column: "ApplicationID",
                principalTable: "Applications",
                principalColumn: "ApplicationID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScalingConfigurations_Environments_EnvironmentID",
                table: "ScalingConfigurations",
                column: "EnvironmentID",
                principalTable: "Environments",
                principalColumn: "EnvironmentID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScalingConfigurations_Applications_ApplicationID",
                table: "ScalingConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_ScalingConfigurations_Environments_EnvironmentID",
                table: "ScalingConfigurations");

            migrationBuilder.AddForeignKey(
                name: "FK_ScalingConfigurations_Applications_ApplicationID",
                table: "ScalingConfigurations",
                column: "ApplicationID",
                principalTable: "Applications",
                principalColumn: "ApplicationID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ScalingConfigurations_Environments_EnvironmentID",
                table: "ScalingConfigurations",
                column: "EnvironmentID",
                principalTable: "Environments",
                principalColumn: "EnvironmentID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
