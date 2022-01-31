using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class addrolemodelrelationrevision : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TB_M_Role_TB_M_Account_AccountNIK",
                table: "TB_M_Role");

            migrationBuilder.DropIndex(
                name: "IX_TB_M_Role_AccountNIK",
                table: "TB_M_Role");

            migrationBuilder.DropColumn(
                name: "AccountNIK",
                table: "TB_M_Role");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountNIK",
                table: "TB_M_Role",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TB_M_Role_AccountNIK",
                table: "TB_M_Role",
                column: "AccountNIK");

            migrationBuilder.AddForeignKey(
                name: "FK_TB_M_Role_TB_M_Account_AccountNIK",
                table: "TB_M_Role",
                column: "AccountNIK",
                principalTable: "TB_M_Account",
                principalColumn: "NIK",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
