using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QMSPOC.Migrations
{
    /// <inheritdoc />
    public partial class Added_ItemMessurement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppItemMessurements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppItemMessurements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppItemMessurements_AppItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "AppItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppItemMessurements_ItemId",
                table: "AppItemMessurements",
                column: "ItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppItemMessurements");
        }
    }
}
