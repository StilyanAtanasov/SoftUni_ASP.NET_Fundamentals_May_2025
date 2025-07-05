using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace RecipeSharingPlatform.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_AspNetUsers_AuthorId",
                table: "Recipes");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_Categories_CategoryId",
                table: "Recipes");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersRecipes_AspNetUsers_UserId",
                table: "UsersRecipes");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersRecipes_Recipes_RecipeId",
                table: "UsersRecipes");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId1",
                table: "Recipes",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4d6f43b3-bc16-43b5-a739-75ab3d60d2ff", "AQAAAAIAAYagAAAAEIAB+04+oSkUDkafeQqzIVof/3pV347wGJg1agM7rzjCdJH7x2NsUOyqAACT8THh1g==", "cf2d36c5-5936-451b-8523-6afa3775ddd8" });

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CategoryId1", "CreatedOn" },
                values: new object[] { null, new DateTime(2025, 7, 5, 12, 17, 58, 392, DateTimeKind.Local).AddTicks(7988) });

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CategoryId1", "CreatedOn" },
                values: new object[] { null, new DateTime(2025, 7, 5, 12, 17, 58, 392, DateTimeKind.Local).AddTicks(8040) });

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CategoryId1", "CreatedOn" },
                values: new object[] { null, new DateTime(2025, 7, 5, 12, 17, 58, 392, DateTimeKind.Local).AddTicks(8043) });

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_CategoryId1",
                table: "Recipes",
                column: "CategoryId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_AspNetUsers_AuthorId",
                table: "Recipes",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_Categories_CategoryId",
                table: "Recipes",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_Categories_CategoryId1",
                table: "Recipes",
                column: "CategoryId1",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersRecipes_AspNetUsers_UserId",
                table: "UsersRecipes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersRecipes_Recipes_RecipeId",
                table: "UsersRecipes",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_AspNetUsers_AuthorId",
                table: "Recipes");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_Categories_CategoryId",
                table: "Recipes");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_Categories_CategoryId1",
                table: "Recipes");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersRecipes_AspNetUsers_UserId",
                table: "UsersRecipes");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersRecipes_Recipes_RecipeId",
                table: "UsersRecipes");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_CategoryId1",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "CategoryId1",
                table: "Recipes");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fafa0ed7-46a7-45ed-8b7b-b6016ca23b8b", "AQAAAAIAAYagAAAAEPJ2+Wy63cMRJHhDdMiHyamW7nGkZ0gxnuYOO/L74y+8xDqw0jAuCb7119MF4w5Q4g==", "73e68096-4485-496c-8d8d-219e03d32b59" });

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2025, 7, 3, 21, 6, 39, 747, DateTimeKind.Local).AddTicks(8994));

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2025, 7, 3, 21, 6, 39, 747, DateTimeKind.Local).AddTicks(9038));

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2025, 7, 3, 21, 6, 39, 747, DateTimeKind.Local).AddTicks(9041));

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_AspNetUsers_AuthorId",
                table: "Recipes",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_Categories_CategoryId",
                table: "Recipes",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersRecipes_AspNetUsers_UserId",
                table: "UsersRecipes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersRecipes_Recipes_RecipeId",
                table: "UsersRecipes",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
