using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Horizons.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDestinationsQueryFilter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7699db7d-964f-4782-8209-d76562e0fece",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2e47e705-96bf-46b6-894f-a6ee1902b20e", "AQAAAAIAAYagAAAAEP7PW1AsysRpV0a9HMayk39WRzZ/4/e1Ow1f43v4WB9jxnZaYqK7PaNc8qGTfI05vQ==", "6c7e498c-c136-4360-aff0-077251fe7e08" });

            migrationBuilder.UpdateData(
                table: "Destinations",
                keyColumn: "Id",
                keyValue: 1,
                column: "PublishedOn",
                value: new DateTime(2025, 7, 3, 9, 37, 34, 444, DateTimeKind.Local).AddTicks(6474));

            migrationBuilder.UpdateData(
                table: "Destinations",
                keyColumn: "Id",
                keyValue: 2,
                column: "PublishedOn",
                value: new DateTime(2025, 7, 3, 9, 37, 34, 444, DateTimeKind.Local).AddTicks(6554));

            migrationBuilder.UpdateData(
                table: "Destinations",
                keyColumn: "Id",
                keyValue: 3,
                column: "PublishedOn",
                value: new DateTime(2025, 7, 3, 9, 37, 34, 444, DateTimeKind.Local).AddTicks(6559));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7699db7d-964f-4782-8209-d76562e0fece",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "049046ab-c190-4a5f-ab28-9364466726e7", "AQAAAAIAAYagAAAAEMfaMO3KlO5DjQ4KOnUEYzyHY4NKnZKMTmF31I4B/OG7EtqodCtUPG3jsdKnwniPpQ==", "dd4da5cd-98a3-4aa8-94ba-98d1eff304b6" });

            migrationBuilder.UpdateData(
                table: "Destinations",
                keyColumn: "Id",
                keyValue: 1,
                column: "PublishedOn",
                value: new DateTime(2025, 7, 2, 11, 33, 40, 513, DateTimeKind.Local).AddTicks(5772));

            migrationBuilder.UpdateData(
                table: "Destinations",
                keyColumn: "Id",
                keyValue: 2,
                column: "PublishedOn",
                value: new DateTime(2025, 7, 2, 11, 33, 40, 513, DateTimeKind.Local).AddTicks(5841));

            migrationBuilder.UpdateData(
                table: "Destinations",
                keyColumn: "Id",
                keyValue: 3,
                column: "PublishedOn",
                value: new DateTime(2025, 7, 2, 11, 33, 40, 513, DateTimeKind.Local).AddTicks(5846));
        }
    }
}
