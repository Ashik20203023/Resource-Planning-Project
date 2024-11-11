using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tesvolt.mutator.konsole.Migrations
{
    /// <inheritdoc />
    public partial class AddModuleFieldToResourcePlanTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ModuleNameId",
                table: "ResourcePlanModels",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModuleNameId",
                table: "ResourcePlanModels");
        }
    }
}
