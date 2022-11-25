using Microsoft.EntityFrameworkCore.Migrations;

namespace Human_Resources_Management.REPOSITORIES.Migrations
{
    public partial class init3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Packages_PackageID",
                table: "Companies");

            migrationBuilder.AlterColumn<int>(
                name: "PackageID",
                table: "Companies",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Packages_PackageID",
                table: "Companies",
                column: "PackageID",
                principalTable: "Packages",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Packages_PackageID",
                table: "Companies");

            migrationBuilder.AlterColumn<int>(
                name: "PackageID",
                table: "Companies",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Packages_PackageID",
                table: "Companies",
                column: "PackageID",
                principalTable: "Packages",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
