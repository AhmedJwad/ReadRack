using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReadRack.Backend.Migrations
{
    /// <inheritdoc />
    public partial class remocecoldepaertment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "colDepartments");

            migrationBuilder.AddColumn<int>(
                name: "CollegeId",
                table: "departments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_departments_CollegeId",
                table: "departments",
                column: "CollegeId");

            migrationBuilder.AddForeignKey(
                name: "FK_departments_colleges_CollegeId",
                table: "departments",
                column: "CollegeId",
                principalTable: "colleges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_departments_colleges_CollegeId",
                table: "departments");

            migrationBuilder.DropIndex(
                name: "IX_departments_CollegeId",
                table: "departments");

            migrationBuilder.DropColumn(
                name: "CollegeId",
                table: "departments");

            migrationBuilder.CreateTable(
                name: "colDepartments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CollegeId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_colDepartments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_colDepartments_colleges_CollegeId",
                        column: x => x.CollegeId,
                        principalTable: "colleges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_colDepartments_departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_colDepartments_CollegeId",
                table: "colDepartments",
                column: "CollegeId");

            migrationBuilder.CreateIndex(
                name: "IX_colDepartments_DepartmentId",
                table: "colDepartments",
                column: "DepartmentId");
        }
    }
}
