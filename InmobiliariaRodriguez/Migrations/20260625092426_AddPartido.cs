using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InmobiliariaRodriguez.Migrations
{
    /// <inheritdoc />
    public partial class AddPartido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Ciudad",
                table: "Propiedades",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Partido",
                table: "Propiedades",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Partido",
                table: "Propiedades");

            migrationBuilder.AlterColumn<int>(
                name: "Ciudad",
                table: "Propiedades",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}
