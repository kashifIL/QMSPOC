using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QMSPOC.Migrations
{
    /// <inheritdoc />
    public partial class Added_ItemMeasuremetnDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppItemMeasuremetnDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemMessurementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Uom = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppItemMeasuremetnDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppItemMeasuremetnDetails_AppItemMessurements_ItemMessurementId",
                        column: x => x.ItemMessurementId,
                        principalTable: "AppItemMessurements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppItemMeasuremetnDetails_ItemMessurementId",
                table: "AppItemMeasuremetnDetails",
                column: "ItemMessurementId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppItemMeasuremetnDetails");
        }
    }
}
