using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable
namespace ServiceScalingDb.Migrations
{
	/// <inheritdoc />
	public partial class instancecountfunction : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION get_number_of_instances(
                    applicationName VARCHAR,
                    projectName VARCHAR,
                    environmentName VARCHAR
                )
                RETURNS SETOF integer AS $$
                BEGIN
                    RETURN QUERY
                    SELECT SC.""NumberOfInstances""
                    FROM public.""ScalingConfigurations"" AS SC
                    INNER JOIN public.""Applications"" AS A ON SC.""ApplicationID"" = A.""ApplicationID""
                    INNER JOIN public.""Projects"" AS P ON A.""ProjectID"" = P.""ProjectID""
                    INNER JOIN public.""Environments"" AS E ON SC.""EnvironmentID"" = E.""EnvironmentID""
                    WHERE A.""ApplicationName"" = applicationName
                    AND P.""ProjectName"" = projectName
                    AND E.""EnvironmentName"" = environmentName;
                END;
                $$ LANGUAGE plpgsql;
            ");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(@"
                DROP FUNCTION IF EXISTS get_number_of_instances(VARCHAR, VARCHAR, VARCHAR);
            ");
		}
	}
}