using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Human_Resources_Management.REPOSITORIES.Migrations
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vacation",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VacationStartDate = table.Column<DateTime>(type: "date", nullable: false),
                    VacationEndDate = table.Column<DateTime>(type: "date", nullable: false),
                    VacationRequestDate = table.Column<DateTime>(type: "date", nullable: false),
                    VacationResponseDate = table.Column<DateTime>(type: "date", nullable: true),
                    AproveStatus = table.Column<int>(type: "int", nullable: false),
                    VacationType = table.Column<int>(type: "int", nullable: false),
                    PersonID = table.Column<int>(type: "int", nullable: false),
                    PhotoName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vacation", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Vacation_Employees_PersonID",
                        column: x => x.PersonID,
                        principalTable: "Employees",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vacation_PersonID",
                table: "Vacation",
                column: "PersonID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vacation");
        }
    }
}
