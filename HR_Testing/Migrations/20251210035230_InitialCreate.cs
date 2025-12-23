using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hr_Testing.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Branch",
                columns: table => new
                {
                    branchId = table.Column<int>(type: "int", nullable: false),
                    branchName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    phoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    companyId = table.Column<int>(type: "int", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: true),
                    createdOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    updatedOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    deletedOn = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branch", x => x.branchId);
                });

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    CompanyId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JoinDate = table.Column<DateOnly>(type: "date", nullable: true),
                    PrimaryPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OtherPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.CompanyId);
                });

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    departmentId = table.Column<int>(type: "int", nullable: false),
                    departmentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.departmentId);
                });

            migrationBuilder.CreateTable(
                name: "employee",
                columns: table => new
                {
                    employeeId = table.Column<int>(type: "int", nullable: false),
                    fullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    joinDate = table.Column<DateOnly>(type: "date", nullable: true),
                    dateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    gender = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    nrc = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employee", x => x.employeeId);
                });

            migrationBuilder.CreateTable(
                name: "Position",
                columns: table => new
                {
                    positionId = table.Column<int>(type: "int", nullable: false),
                    positionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    departmentId = table.Column<int>(type: "int", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: true),
                    createdOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    updatedOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    deletedOn = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Position", x => x.positionId);
                });

            migrationBuilder.CreateTable(
                name: "State",
                columns: table => new
                {
                    StateId = table.Column<int>(type: "int", nullable: false),
                    StateName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_State", x => x.StateId);
                });

            migrationBuilder.CreateTable(
                name: "Street",
                columns: table => new
                {
                    StreetId = table.Column<int>(type: "int", nullable: false),
                    TownshipId = table.Column<int>(type: "int", nullable: true),
                    StreetName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Street", x => x.StreetId);
                });

            migrationBuilder.CreateTable(
                name: "Township",
                columns: table => new
                {
                    TownshipId = table.Column<int>(type: "int", nullable: false),
                    StateId = table.Column<int>(type: "int", nullable: true),
                    TownshipName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Township", x => x.TownshipId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Branch");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.DropTable(
                name: "employee");

            migrationBuilder.DropTable(
                name: "Position");

            migrationBuilder.DropTable(
                name: "State");

            migrationBuilder.DropTable(
                name: "Street");

            migrationBuilder.DropTable(
                name: "Township");
        }
    }
}
