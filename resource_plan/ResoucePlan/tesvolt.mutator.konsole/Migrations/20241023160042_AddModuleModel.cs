using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tesvolt.mutator.konsole.Migrations
{
    /// <inheritdoc />
    public partial class AddModuleModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ModuleModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Modules = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameId = table.Column<int>(type: "int", nullable: false),
                    WorkStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WorkEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NoOfDaysNeeded = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModuleModels_ProjectModels_NameId",
                        column: x => x.NameId,
                        principalTable: "ProjectModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModuleModels_NameId",
                table: "ModuleModels",
                column: "NameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModuleModels");
        }
    }
}
