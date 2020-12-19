using Microsoft.EntityFrameworkCore.Migrations;

namespace SimpsonApp.Migrations
{
    public partial class NewColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "Characters");

            migrationBuilder.AlterColumn<bool>(
                name: "isProta",
                table: "Characters",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "Age",
                table: "Characters",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Occupation",
                table: "Characters",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "appearingSeason",
                table: "Characters",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Occupation",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "appearingSeason",
                table: "Characters");

            migrationBuilder.AlterColumn<bool>(
                name: "isProta",
                table: "Characters",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Age",
                table: "Characters",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "Characters",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
