﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantApp.Migrations
{
    /// <inheritdoc />
    public partial class AddIsTodaySpecialToItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTodaySpecial",
                table: "Items",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTodaySpecial",
                table: "Items");
        }
    }
}
