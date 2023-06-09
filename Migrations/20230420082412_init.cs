using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Academy2023.Net.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarCategories",
                columns: table => new
                {
                    CarCategoryID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CarName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarCategories", x => x.CarCategoryID);
                });

            migrationBuilder.CreateTable(
                name: "FuelTypes",
                columns: table => new
                {
                    FuelTypeID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FuelName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelTypes", x => x.FuelTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Genders",
                columns: table => new
                {
                    GenderID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GenderName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genders", x => x.GenderID);
                });

            migrationBuilder.CreateTable(
                name: "Pictures",
                columns: table => new
                {
                    PictureID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PictureName = table.Column<string>(type: "TEXT", nullable: false),
                    RawData = table.Column<byte[]>(type: "BLOB", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pictures", x => x.PictureID);
                });

            migrationBuilder.CreateTable(
                name: "cars",
                columns: table => new
                {
                    CarID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserDataID = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxSeat = table.Column<int>(type: "INTEGER", nullable: false),
                    RegNum = table.Column<string>(type: "TEXT", nullable: false),
                    FuelTypeID = table.Column<int>(type: "INTEGER", nullable: false),
                    CarCategoryID = table.Column<int>(type: "INTEGER", nullable: false),
                    TrunkAvailable = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cars", x => x.CarID);
                    table.ForeignKey(
                        name: "FK_cars_CarCategories_CarCategoryID",
                        column: x => x.CarCategoryID,
                        principalTable: "CarCategories",
                        principalColumn: "CarCategoryID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cars_FuelTypes_FuelTypeID",
                        column: x => x.FuelTypeID,
                        principalTable: "FuelTypes",
                        principalColumn: "FuelTypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rides",
                columns: table => new
                {
                    RideId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Departure = table.Column<string>(type: "TEXT", nullable: false),
                    Arrival = table.Column<string>(type: "TEXT", nullable: false),
                    DepartureTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ArrivalTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Duration = table.Column<int>(type: "INTEGER", nullable: false),
                    Km = table.Column<double>(type: "REAL", nullable: false),
                    AvailableSeat = table.Column<int>(type: "INTEGER", nullable: false),
                    CarID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rides", x => x.RideId);
                    table.ForeignKey(
                        name: "FK_Rides_cars_CarID",
                        column: x => x.CarID,
                        principalTable: "cars",
                        principalColumn: "CarID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "usersData",
                columns: table => new
                {
                    UserDataID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    GenderID = table.Column<int>(type: "INTEGER", nullable: false),
                    BirthDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    CF = table.Column<string>(type: "TEXT", nullable: false),
                    License = table.Column<string>(type: "TEXT", nullable: false),
                    AuthID = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    HasCar = table.Column<bool>(type: "INTEGER", nullable: false),
                    PictureID = table.Column<int>(type: "INTEGER", nullable: true),
                    RideId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usersData", x => x.UserDataID);
                    table.ForeignKey(
                        name: "FK_usersData_Genders_GenderID",
                        column: x => x.GenderID,
                        principalTable: "Genders",
                        principalColumn: "GenderID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_usersData_Pictures_PictureID",
                        column: x => x.PictureID,
                        principalTable: "Pictures",
                        principalColumn: "PictureID");
                    table.ForeignKey(
                        name: "FK_usersData_Rides_RideId",
                        column: x => x.RideId,
                        principalTable: "Rides",
                        principalColumn: "RideId");
                });

            migrationBuilder.CreateTable(
                name: "userDataRides",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserDataID = table.Column<int>(type: "INTEGER", nullable: false),
                    RideId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userDataRides", x => x.ID);
                    table.ForeignKey(
                        name: "FK_userDataRides_Rides_RideId",
                        column: x => x.RideId,
                        principalTable: "Rides",
                        principalColumn: "RideId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_userDataRides_usersData_UserDataID",
                        column: x => x.UserDataID,
                        principalTable: "usersData",
                        principalColumn: "UserDataID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cars_CarCategoryID",
                table: "cars",
                column: "CarCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_cars_FuelTypeID",
                table: "cars",
                column: "FuelTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_cars_UserDataID",
                table: "cars",
                column: "UserDataID");

            migrationBuilder.CreateIndex(
                name: "IX_Rides_CarID",
                table: "Rides",
                column: "CarID");

            migrationBuilder.CreateIndex(
                name: "IX_userDataRides_RideId",
                table: "userDataRides",
                column: "RideId");

            migrationBuilder.CreateIndex(
                name: "IX_userDataRides_UserDataID",
                table: "userDataRides",
                column: "UserDataID");

            migrationBuilder.CreateIndex(
                name: "IX_usersData_GenderID",
                table: "usersData",
                column: "GenderID");

            migrationBuilder.CreateIndex(
                name: "IX_usersData_PictureID",
                table: "usersData",
                column: "PictureID");

            migrationBuilder.CreateIndex(
                name: "IX_usersData_RideId",
                table: "usersData",
                column: "RideId");

            migrationBuilder.AddForeignKey(
                name: "FK_cars_usersData_UserDataID",
                table: "cars",
                column: "UserDataID",
                principalTable: "usersData",
                principalColumn: "UserDataID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cars_CarCategories_CarCategoryID",
                table: "cars");

            migrationBuilder.DropForeignKey(
                name: "FK_cars_FuelTypes_FuelTypeID",
                table: "cars");

            migrationBuilder.DropForeignKey(
                name: "FK_cars_usersData_UserDataID",
                table: "cars");

            migrationBuilder.DropTable(
                name: "userDataRides");

            migrationBuilder.DropTable(
                name: "CarCategories");

            migrationBuilder.DropTable(
                name: "FuelTypes");

            migrationBuilder.DropTable(
                name: "usersData");

            migrationBuilder.DropTable(
                name: "Genders");

            migrationBuilder.DropTable(
                name: "Pictures");

            migrationBuilder.DropTable(
                name: "Rides");

            migrationBuilder.DropTable(
                name: "cars");
        }
    }
}
