using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReadRack.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AdddepartmentAfterupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_departments", x => x.Id);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_departments_Name",
                table: "departments",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "colDepartments");

            migrationBuilder.DropTable(
                name: "departments");
        }
    }
}
