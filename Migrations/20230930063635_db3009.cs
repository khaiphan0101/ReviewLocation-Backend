using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace review.Migrations
{
    public partial class db3009 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    ID = table.Column<string>(type: "varchar(60)", nullable: false),
                    UserName = table.Column<string>(type: "varchar(60)", nullable: false),
                    Password = table.Column<string>(type: "varchar(100)", nullable: false),
                    Role = table.Column<string>(type: "varchar(10)", nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    ID = table.Column<string>(type: "varchar(60)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Province",
                columns: table => new
                {
                    ID = table.Column<string>(type: "varchar(60)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Province", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Profile",
                columns: table => new
                {
                    ID = table.Column<string>(type: "varchar(60)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Phone = table.Column<string>(type: "varchar(12)", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: true),
                    Avatar = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    Identify = table.Column<string>(type: "varchar(100)", nullable: false),
                    AccountID = table.Column<string>(type: "varchar(60)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profile", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Profile_Account_AccountID",
                        column: x => x.AccountID,
                        principalTable: "Account",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefeshToken",
                columns: table => new
                {
                    ID = table.Column<string>(type: "varchar(60)", nullable: false),
                    RefeshToken = table.Column<string>(type: "varchar(200)", nullable: false),
                    ExpiredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccountID = table.Column<string>(type: "varchar(60)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefeshToken", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RefeshToken_Account_AccountID",
                        column: x => x.AccountID,
                        principalTable: "Account",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProvinceCategory",
                columns: table => new
                {
                    ID = table.Column<string>(type: "varchar(60)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Thumb = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    ProvinceID = table.Column<string>(type: "varchar(60)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProvinceCategory", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProvinceCategory_Province_ProvinceID",
                        column: x => x.ProvinceID,
                        principalTable: "Province",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProfileFollow",
                columns: table => new
                {
                    ID = table.Column<string>(type: "varchar(60)", nullable: false),
                    FollowingID = table.Column<string>(type: "varchar(60)", nullable: false),
                    FollowerID = table.Column<string>(type: "varchar(60)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileFollow", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProfileFollow_Profile_FollowerID",
                        column: x => x.FollowerID,
                        principalTable: "Profile",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ProfileFollow_Profile_FollowingID",
                        column: x => x.FollowingID,
                        principalTable: "Profile",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Profile_AccountID",
                table: "Profile",
                column: "AccountID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProfileFollow_FollowerID",
                table: "ProfileFollow",
                column: "FollowerID");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileFollow_FollowingID",
                table: "ProfileFollow",
                column: "FollowingID");

            migrationBuilder.CreateIndex(
                name: "IX_ProvinceCategory_ProvinceID",
                table: "ProvinceCategory",
                column: "ProvinceID");

            migrationBuilder.CreateIndex(
                name: "IX_RefeshToken_AccountID",
                table: "RefeshToken",
                column: "AccountID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "ProfileFollow");

            migrationBuilder.DropTable(
                name: "ProvinceCategory");

            migrationBuilder.DropTable(
                name: "RefeshToken");

            migrationBuilder.DropTable(
                name: "Profile");

            migrationBuilder.DropTable(
                name: "Province");

            migrationBuilder.DropTable(
                name: "Account");
        }
    }
}
