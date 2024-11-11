using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tesvolt.mutator.konsole.Migrations
{
    /// <inheritdoc />
    public partial class Initialcreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResourceModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    Skills = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssetName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PoNameId = table.Column<int>(type: "int", nullable: false),
                    ProjectStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectModels_ResourceModels_PoNameId",
                        column: x => x.PoNameId,
                        principalTable: "ResourceModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VacationPlanModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResourceModelId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NoOfDay = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VacationPlanModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VacationPlanModels_ResourceModels_ResourceModelId",
                        column: x => x.ResourceModelId,
                        principalTable: "ResourceModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AttendanceEntryModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResourceNameId = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProjectNameId = table.Column<int>(type: "int", nullable: true),
                    TaskType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceEntryModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttendanceEntryModels_ProjectModels_ProjectNameId",
                        column: x => x.ProjectNameId,
                        principalTable: "ProjectModels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AttendanceEntryModels_ResourceModels_ResourceNameId",
                        column: x => x.ResourceNameId,
                        principalTable: "ResourceModels",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ResourcePlanModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResourceNameId = table.Column<int>(type: "int", nullable: true),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    ProjectNameId = table.Column<int>(type: "int", nullable: false),
                    Percentage = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourcePlanModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResourcePlanModels_ProjectModels_ProjectNameId",
                        column: x => x.ProjectNameId,
                        principalTable: "ProjectModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResourcePlanModels_ResourceModels_ResourceNameId",
                        column: x => x.ResourceNameId,
                        principalTable: "ResourceModels",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AttendanceDetailEntryModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResourceNameId = table.Column<int>(type: "int", nullable: true),
                    AttendanceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AttendanceEntryId = table.Column<int>(type: "int", nullable: false),
                    ProjectNameId = table.Column<int>(type: "int", nullable: true),
                    TaskType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceDetailEntryModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttendanceDetailEntryModels_AttendanceEntryModels_AttendanceEntryId",
                        column: x => x.AttendanceEntryId,
                        principalTable: "AttendanceEntryModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttendanceDetailEntryModels_ProjectModels_ProjectNameId",
                        column: x => x.ProjectNameId,
                        principalTable: "ProjectModels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AttendanceDetailEntryModels_ResourceModels_ResourceNameId",
                        column: x => x.ResourceNameId,
                        principalTable: "ResourceModels",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceDetailEntryModels_AttendanceEntryId",
                table: "AttendanceDetailEntryModels",
                column: "AttendanceEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceDetailEntryModels_ProjectNameId",
                table: "AttendanceDetailEntryModels",
                column: "ProjectNameId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceDetailEntryModels_ResourceNameId",
                table: "AttendanceDetailEntryModels",
                column: "ResourceNameId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceEntryModels_ProjectNameId",
                table: "AttendanceEntryModels",
                column: "ProjectNameId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceEntryModels_ResourceNameId",
                table: "AttendanceEntryModels",
                column: "ResourceNameId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectModels_PoNameId",
                table: "ProjectModels",
                column: "PoNameId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourcePlanModels_ProjectNameId",
                table: "ResourcePlanModels",
                column: "ProjectNameId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourcePlanModels_ResourceNameId",
                table: "ResourcePlanModels",
                column: "ResourceNameId");

            migrationBuilder.CreateIndex(
                name: "IX_VacationPlanModels_ResourceModelId",
                table: "VacationPlanModels",
                column: "ResourceModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttendanceDetailEntryModels");

            migrationBuilder.DropTable(
                name: "ResourcePlanModels");

            migrationBuilder.DropTable(
                name: "VacationPlanModels");

            migrationBuilder.DropTable(
                name: "AttendanceEntryModels");

            migrationBuilder.DropTable(
                name: "ProjectModels");

            migrationBuilder.DropTable(
                name: "ResourceModels");
        }
    }
}
