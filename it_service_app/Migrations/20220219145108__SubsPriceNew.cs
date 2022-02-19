using Microsoft.EntityFrameworkCore.Migrations;

namespace it_service_app.Migrations
{
    public partial class _SubsPriceNew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Descripiton",
                table: "subscriptionTypes",
                newName: "Description");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "subscriptionTypes",
                newName: "Descripiton");
        }
    }
}
