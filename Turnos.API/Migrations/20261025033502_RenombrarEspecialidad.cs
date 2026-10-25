using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Turnos.API.Migrations
{
    /// <inheritdoc />
    public partial class RenombrarEspecialidad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Profesionales",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Publicidad",
                table: "Profesionales",
                newName: "Especialidad");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Profesionales",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Especialidad",
                table: "Profesionales",
                newName: "Publicidad");
        }
    }
}
