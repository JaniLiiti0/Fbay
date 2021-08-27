using Microsoft.EntityFrameworkCore.Migrations;

namespace Fbay.Migrations
{
    public partial class defaultImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Listings");
        }
    }
}
