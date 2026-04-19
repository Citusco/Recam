using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Remp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addselectedmediaassets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SelectedMedias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MediaAssetId = table.Column<int>(type: "int", nullable: false),
                    ListingCaseId = table.Column<int>(type: "int", nullable: false),
                    AgentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SelectedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectedMedias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SelectedMedias_Agents_AgentId",
                        column: x => x.AgentId,
                        principalTable: "Agents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SelectedMedias_ListingCases_ListingCaseId",
                        column: x => x.ListingCaseId,
                        principalTable: "ListingCases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SelectedMedias_MediaAssets_MediaAssetId",
                        column: x => x.MediaAssetId,
                        principalTable: "MediaAssets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SelectedMedias_AgentId",
                table: "SelectedMedias",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_SelectedMedias_ListingCaseId",
                table: "SelectedMedias",
                column: "ListingCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_SelectedMedias_MediaAssetId",
                table: "SelectedMedias",
                column: "MediaAssetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SelectedMedias");
        }
    }
}
