using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GardenSpaceService.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BaseBranchType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BaseRootTypeId = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseBranchType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BaseRootType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RootId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseRootType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GardenSpace",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatorId = table.Column<int>(type: "int", nullable: false),
                    SpaceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    SpaceTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPrivate = table.Column<bool>(type: "bit", nullable: false),
                    OnlyInvite = table.Column<bool>(type: "bit", nullable: false),
                    PlanStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PlanEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GardenSpace", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GardenRootType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GardenSpaceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GardenRootType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GardenRootType_GardenSpace_GardenSpaceId",
                        column: x => x.GardenSpaceId,
                        principalTable: "GardenSpace",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GardenSpaceUserMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GardenSpaceId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    ParticiDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GardenSpaceUserMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GardenSpaceUserMap_GardenSpace_GardenSpaceId",
                        column: x => x.GardenSpaceId,
                        principalTable: "GardenSpace",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GardenBranchType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RootTypeId = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GardenBranchType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GardenBranchType_GardenRootType_RootTypeId",
                        column: x => x.RootTypeId,
                        principalTable: "GardenRootType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GardenBranchType_RootTypeId",
                table: "GardenBranchType",
                column: "RootTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GardenRootType_GardenSpaceId",
                table: "GardenRootType",
                column: "GardenSpaceId");

            migrationBuilder.CreateIndex(
                name: "IX_GardenSpaceUserMap_GardenSpaceId",
                table: "GardenSpaceUserMap",
                column: "GardenSpaceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BaseBranchType");

            migrationBuilder.DropTable(
                name: "BaseRootType");

            migrationBuilder.DropTable(
                name: "GardenBranchType");

            migrationBuilder.DropTable(
                name: "GardenSpaceUserMap");

            migrationBuilder.DropTable(
                name: "GardenRootType");

            migrationBuilder.DropTable(
                name: "GardenSpace");
        }
    }
}
