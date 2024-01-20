using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceScalingDb.Migrations
{
	/// <inheritdoc />
	public partial class makeprojectnameunique : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateIndex(
				name: "IX_Projects_ProjectName",
				table: "Projects",
				column: "ProjectName",
				unique: true);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropIndex(
				name: "IX_Projects_ProjectName",
				table: "Projects");
		}
	}
}