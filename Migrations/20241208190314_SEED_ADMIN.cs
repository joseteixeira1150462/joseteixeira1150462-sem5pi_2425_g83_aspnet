using HealthCare.Domain.Shared;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthCare.Migrations
{
    /// <inheritdoc />
    public partial class SEED_ADMIN : Migration
    {
        private string AdminId = "c232958c-5b3f-456e-a17e-a63f485a7570";

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "INSERT INTO Users VALUES (" +
                    $"'{AdminId}'," +
                    $"'{StringHasher.GetHashString("sem5pi_2425_G83")}'," +
                    "'admin@HealthCare.com'," +
                    " 'Admin'," +
                    "1," +
                    "5," +
                    "'2024-10-27 14:30:00'" +
                ")"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                $"DELETE FROM Users WHERE Id = '{AdminId}'"
            );
        }
    }
}