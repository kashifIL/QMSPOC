using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QMSPOC.Migrations
{
    /// <inheritdoc />
    public partial class Added_ItemBomDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppItemBomDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemBomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Qty = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Uom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppItemBomDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppItemBomDetails_AppItemBoms_ItemBomId",
                        column: x => x.ItemBomId,
                        principalTable: "AppItemBoms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppItemBomDetails_AppItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "AppItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppItemBomDetails_ItemBomId",
                table: "AppItemBomDetails",
                column: "ItemBomId");

            migrationBuilder.CreateIndex(
                name: "IX_AppItemBomDetails_ItemId",
                table: "AppItemBomDetails",
                column: "ItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppItemBomDetails");
        }
    }
}
