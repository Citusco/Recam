using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recam.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class fixFkBugsAndAddDbsets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaseContact_ListingCases_ListingCaseId",
                table: "CaseContact");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaAsset_AspNetUsers_UserId",
                table: "MediaAsset");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaAsset_ListingCases_ListingCaseId",
                table: "MediaAsset");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MediaAsset",
                table: "MediaAsset");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CaseContact",
                table: "CaseContact");

            migrationBuilder.RenameTable(
                name: "MediaAsset",
                newName: "MediaAssets");

            migrationBuilder.RenameTable(
                name: "CaseContact",
                newName: "CaseContacts");

            migrationBuilder.RenameIndex(
                name: "IX_MediaAsset_UserId",
                table: "MediaAssets",
                newName: "IX_MediaAssets_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_MediaAsset_ListingCaseId",
                table: "MediaAssets",
                newName: "IX_MediaAssets_ListingCaseId");

            migrationBuilder.RenameIndex(
                name: "IX_CaseContact_ListingCaseId",
                table: "CaseContacts",
                newName: "IX_CaseContacts_ListingCaseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MediaAssets",
                table: "MediaAssets",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CaseContacts",
                table: "CaseContacts",
                column: "ContactId");

            migrationBuilder.CreateTable(
                name: "PhotographyCompanies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PhotographyCompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotographyCompanies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhotographyCompanies_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AgentPhotographyCompanies",
                columns: table => new
                {
                    AgentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PhotographyCompanyId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgentPhotographyCompanies", x => new { x.AgentId, x.PhotographyCompanyId });
                    table.ForeignKey(
                        name: "FK_AgentPhotographyCompanies_Agents_AgentId",
                        column: x => x.AgentId,
                        principalTable: "Agents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AgentPhotographyCompanies_PhotographyCompanies_PhotographyCompanyId",
                        column: x => x.PhotographyCompanyId,
                        principalTable: "PhotographyCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AgentPhotographyCompanies_PhotographyCompanyId",
                table: "AgentPhotographyCompanies",
                column: "PhotographyCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CaseContacts_ListingCases_ListingCaseId",
                table: "CaseContacts",
                column: "ListingCaseId",
                principalTable: "ListingCases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MediaAssets_AspNetUsers_UserId",
                table: "MediaAssets",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MediaAssets_ListingCases_ListingCaseId",
                table: "MediaAssets",
                column: "ListingCaseId",
                principalTable: "ListingCases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaseContacts_ListingCases_ListingCaseId",
                table: "CaseContacts");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaAssets_AspNetUsers_UserId",
                table: "MediaAssets");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaAssets_ListingCases_ListingCaseId",
                table: "MediaAssets");

            migrationBuilder.DropTable(
                name: "AgentPhotographyCompanies");

            migrationBuilder.DropTable(
                name: "PhotographyCompanies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MediaAssets",
                table: "MediaAssets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CaseContacts",
                table: "CaseContacts");

            migrationBuilder.RenameTable(
                name: "MediaAssets",
                newName: "MediaAsset");

            migrationBuilder.RenameTable(
                name: "CaseContacts",
                newName: "CaseContact");

            migrationBuilder.RenameIndex(
                name: "IX_MediaAssets_UserId",
                table: "MediaAsset",
                newName: "IX_MediaAsset_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_MediaAssets_ListingCaseId",
                table: "MediaAsset",
                newName: "IX_MediaAsset_ListingCaseId");

            migrationBuilder.RenameIndex(
                name: "IX_CaseContacts_ListingCaseId",
                table: "CaseContact",
                newName: "IX_CaseContact_ListingCaseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MediaAsset",
                table: "MediaAsset",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CaseContact",
                table: "CaseContact",
                column: "ContactId");

            migrationBuilder.AddForeignKey(
                name: "FK_CaseContact_ListingCases_ListingCaseId",
                table: "CaseContact",
                column: "ListingCaseId",
                principalTable: "ListingCases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MediaAsset_AspNetUsers_UserId",
                table: "MediaAsset",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MediaAsset_ListingCases_ListingCaseId",
                table: "MediaAsset",
                column: "ListingCaseId",
                principalTable: "ListingCases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
