using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookArchive.Migrations
{
    /// <inheritdoc />
    public partial class CreateAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Roles (Name) VALUES ('admin')", true);
            migrationBuilder.Sql("SET @RoleId = LAST_INSERT_ID()", true);
            migrationBuilder.Sql("INSERT INTO Users (Email, Password, RoleId) VALUES ('admin@admin.com', 'admin', @RoleId)", true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
