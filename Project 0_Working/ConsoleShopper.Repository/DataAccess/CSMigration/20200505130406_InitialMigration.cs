using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ConsoleShopper.Repository.DataAccess.CSMigration
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 128, nullable: true),
                    ProductCode = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Inventory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    StoreId = table.Column<int>(nullable: false),
                    LoggedUserId = table.Column<int>(nullable: false),
                    ChangedDate = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inventory_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inventory_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(maxLength: 128, nullable: true),
                    LastName = table.Column<string>(maxLength: 128, nullable: true),
                    Email = table.Column<string>(maxLength: 128, nullable: true),
                    PhoneNo = table.Column<string>(maxLength: 128, nullable: true),
                    Password = table.Column<string>(maxLength: 128, nullable: true),
                    UserTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_UserTypes_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "UserTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerAddress",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Street = table.Column<string>(maxLength: 128, nullable: true),
                    City = table.Column<string>(maxLength: 128, nullable: true),
                    State = table.Column<string>(maxLength: 128, nullable: true),
                    Zip = table.Column<string>(maxLength: 128, nullable: true),
                    CustomerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerAddress_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderDate = table.Column<DateTimeOffset>(nullable: true),
                    StoreId = table.Column<int>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderLineItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    OrderId = table.Column<int>(nullable: false),
                    InventoryItemId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderLineItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderLineItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Name", "Price", "ProductCode" },
                values: new object[,]
                {
                    { 1, "Piano", 150.55m, "P00001" },
                    { 10, "Saxophone", 150.55m, "P00010" },
                    { 8, "Bagpipes", 150.55m, "P00008" },
                    { 7, "Guitar", 150.55m, "P00007" },
                    { 6, "Violin", 150.55m, "P00006" },
                    { 9, "Ukulele", 150.55m, "P00009" },
                    { 4, "Piccolo", 150.55m, "P00004" },
                    { 3, "Accordian", 150.55m, "P00003" },
                    { 2, "Flute", 150.55m, "P00002" },
                    { 5, "Trombone", 150.55m, "P00005" }
                });

            migrationBuilder.InsertData(
                table: "Stores",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Florida" },
                    { 2, "New York" },
                    { 3, "Texas" },
                    { 4, "Washington" },
                    { 5, "California" }
                });

            migrationBuilder.InsertData(
                table: "UserTypes",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "Customer" }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Password", "PhoneNo", "UserTypeId" },
                values: new object[,]
                {
                    { 20, "omelrosej@artisteer.com", "Serena", "San", "password", null, 2 },
                    { 1, "lflear0@outlook.com", "Sulav", "Aryal", "password", null, 1 },
                    { 3, "acloney2@dropbox.com", "Brigitte", "Laufer", "password", null, 2 },
                    { 4, "tscurrell3@reuters.com", "Bettie", "Turek", "password", null, 2 },
                    { 5, "mfonzone4@vk.com", "Kenneth", "Windsor", "password", null, 2 },
                    { 6, "igallaccio5@tmall.com", "Maribeth", "Fontenot", "password", null, 2 },
                    { 7, "aasken6@etsy.com", "Barret", "Waltrip", "password", null, 2 },
                    { 8, "dmagrane7@dagondesign.com", "Jeana", "Dunston", "password", null, 2 },
                    { 9, "gdibdale8@nih.gov", "Mirian", "Stroda", "password", null, 2 },
                    { 2, "dcove@networking.org", "Danyelle", "Tsosie", "password", null, 2 },
                    { 11, "ymartyna@ebay.com", "Lucilla", "Chang", "password", null, 2 },
                    { 10, "acockran9@arizona.edu", "Beverley", "Digangi", "password", null, 2 },
                    { 18, "mpeyroh@foxnews.com", "Althea", "Dent", "password", null, 2 },
                    { 17, "tbarentsg@independent.co", "Susy", "Argo", "password", null, 2 },
                    { 16, "ewigginf@skyrock.com", "Mireya", "Pierro", "password", null, 2 },
                    { 19, "jarnaudini@webmd.com", "Rosana", "Purvis", "password", null, 2 },
                    { 14, "aharborowd@nbcnews.com", "Moises", "Meche", "password", null, 2 },
                    { 13, "bianizzic@wisc.edu", "Taneka", "Ord", "password", null, 2 },
                    { 12, "cbmccaughenb@umn.com", "Gigi", "Degree", "password", null, 2 },
                    { 15, "nellyatte@homestead.com", "Hans", "Spurgin", "password", null, 2 }
                });

            migrationBuilder.InsertData(
                table: "Inventory",
                columns: new[] { "Id", "ChangedDate", "LoggedUserId", "ProductId", "Quantity", "StoreId" },
                values: new object[,]
                {
                    { 37, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(7196), new TimeSpan(0, -7, 0, 0, 0)), 1, 7, 20, 4 },
                    { 38, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(7202), new TimeSpan(0, -7, 0, 0, 0)), 1, 8, 20, 4 },
                    { 39, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(7207), new TimeSpan(0, -7, 0, 0, 0)), 1, 9, 20, 4 },
                    { 40, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(7213), new TimeSpan(0, -7, 0, 0, 0)), 1, 10, 20, 4 },
                    { 41, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(7219), new TimeSpan(0, -7, 0, 0, 0)), 1, 1, 20, 5 },
                    { 42, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(7224), new TimeSpan(0, -7, 0, 0, 0)), 1, 2, 20, 5 },
                    { 44, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(7236), new TimeSpan(0, -7, 0, 0, 0)), 1, 4, 20, 5 },
                    { 45, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(7242), new TimeSpan(0, -7, 0, 0, 0)), 1, 5, 20, 5 },
                    { 46, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(7247), new TimeSpan(0, -7, 0, 0, 0)), 1, 6, 20, 5 },
                    { 48, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(7259), new TimeSpan(0, -7, 0, 0, 0)), 1, 8, 20, 5 },
                    { 49, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(7265), new TimeSpan(0, -7, 0, 0, 0)), 1, 9, 20, 5 },
                    { 50, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(7270), new TimeSpan(0, -7, 0, 0, 0)), 1, 10, 20, 5 },
                    { 36, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(7190), new TimeSpan(0, -7, 0, 0, 0)), 1, 6, 20, 4 },
                    { 43, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(7230), new TimeSpan(0, -7, 0, 0, 0)), 1, 3, 20, 5 },
                    { 47, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(7253), new TimeSpan(0, -7, 0, 0, 0)), 1, 7, 20, 5 },
                    { 35, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(7184), new TimeSpan(0, -7, 0, 0, 0)), 1, 5, 20, 4 },
                    { 33, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(7173), new TimeSpan(0, -7, 0, 0, 0)), 1, 3, 20, 4 },
                    { 2, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(6948), new TimeSpan(0, -7, 0, 0, 0)), 1, 2, 20, 1 },
                    { 3, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(6985), new TimeSpan(0, -7, 0, 0, 0)), 1, 3, 20, 1 },
                    { 4, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(6993), new TimeSpan(0, -7, 0, 0, 0)), 1, 4, 20, 1 },
                    { 5, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(6999), new TimeSpan(0, -7, 0, 0, 0)), 1, 5, 20, 1 },
                    { 6, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(7005), new TimeSpan(0, -7, 0, 0, 0)), 1, 6, 20, 1 },
                    { 7, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(7010), new TimeSpan(0, -7, 0, 0, 0)), 1, 7, 20, 1 },
                    { 8, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(7016), new TimeSpan(0, -7, 0, 0, 0)), 1, 8, 20, 1 },
                    { 9, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(7022), new TimeSpan(0, -7, 0, 0, 0)), 1, 9, 20, 1 },
                    { 10, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(7028), new TimeSpan(0, -7, 0, 0, 0)), 1, 10, 20, 1 },
                    { 11, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(7033), new TimeSpan(0, -7, 0, 0, 0)), 1, 1, 20, 2 },
                    { 12, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(7039), new TimeSpan(0, -7, 0, 0, 0)), 1, 2, 20, 2 },
                    { 13, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(7045), new TimeSpan(0, -7, 0, 0, 0)), 1, 3, 20, 2 },
                    { 14, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(7051), new TimeSpan(0, -7, 0, 0, 0)), 1, 4, 20, 2 },
                    { 15, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(7056), new TimeSpan(0, -7, 0, 0, 0)), 1, 5, 20, 2 },
                    { 16, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(7062), new TimeSpan(0, -7, 0, 0, 0)), 1, 6, 20, 2 },
                    { 17, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(7070), new TimeSpan(0, -7, 0, 0, 0)), 1, 7, 20, 2 },
                    { 18, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(7075), new TimeSpan(0, -7, 0, 0, 0)), 1, 8, 20, 2 },
                    { 32, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(7167), new TimeSpan(0, -7, 0, 0, 0)), 1, 2, 20, 4 },
                    { 31, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(7162), new TimeSpan(0, -7, 0, 0, 0)), 1, 1, 20, 4 },
                    { 30, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(7156), new TimeSpan(0, -7, 0, 0, 0)), 1, 10, 20, 3 },
                    { 29, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(7138), new TimeSpan(0, -7, 0, 0, 0)), 1, 9, 20, 3 },
                    { 28, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(7132), new TimeSpan(0, -7, 0, 0, 0)), 1, 8, 20, 3 },
                    { 27, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(7127), new TimeSpan(0, -7, 0, 0, 0)), 1, 7, 20, 3 },
                    { 34, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(7179), new TimeSpan(0, -7, 0, 0, 0)), 1, 4, 20, 4 },
                    { 26, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(7121), new TimeSpan(0, -7, 0, 0, 0)), 1, 6, 20, 3 },
                    { 24, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(7110), new TimeSpan(0, -7, 0, 0, 0)), 1, 4, 20, 3 },
                    { 23, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(7104), new TimeSpan(0, -7, 0, 0, 0)), 1, 3, 20, 3 },
                    { 22, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(7098), new TimeSpan(0, -7, 0, 0, 0)), 1, 2, 20, 3 },
                    { 21, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(7093), new TimeSpan(0, -7, 0, 0, 0)), 1, 1, 20, 3 },
                    { 20, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(7087), new TimeSpan(0, -7, 0, 0, 0)), 1, 10, 20, 2 },
                    { 19, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(7081), new TimeSpan(0, -7, 0, 0, 0)), 1, 9, 20, 2 },
                    { 25, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(7115), new TimeSpan(0, -7, 0, 0, 0)), 1, 5, 20, 3 },
                    { 1, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 298, DateTimeKind.Unspecified).AddTicks(675), new TimeSpan(0, -7, 0, 0, 0)), 1, 1, 20, 1 }
                });

            migrationBuilder.InsertData(
                table: "CustomerAddress",
                columns: new[] { "Id", "City", "CustomerId", "State", "Street", "Zip" },
                values: new object[,]
                {
                    { 3, "Fort Worth", 1, "TX", "96 Franklin Ave.", "76110" },
                    { 18, "Eastpointe", 18, "MI", "58 Fifth St.", "48021" },
                    { 17, "Canandaigua", 17, "NY", "206 New Saddle Ave.", "14424" },
                    { 16, "Manahawkin", 16, "NJ", "290 Marsh St. ", "08050" },
                    { 15, "Lancaster", 15, "NY", "41 Buckingham Ave", "14086" },
                    { 14, "Meadow", 14, "NJ", "48 W. Oak St.", "08003" },
                    { 13, "Huntington", 13, "NY", "467 South Smoky Hollow St", "11743" },
                    { 12, "Munster", 12, "IN", "265 Prairie St.", "46321" },
                    { 11, "Wenatchee", 11, "WA", "3 Myers Street", "98801" },
                    { 10, "Green Cove Springs", 10, "FL", "89 North Devonshire Dr", "32043" },
                    { 9, "Roseville", 9, "MI", "84 Woodsman St.", "48066" },
                    { 8, "West Palm Beach", 8, "FL", "37 Pilgrim Lane", "33404" },
                    { 7, "Missoula", 7, "MT", "580 West Deerfield Road", "59801" },
                    { 1, "Aberdeen", 6, "SD", "67 Carriage Drive", "57401" },
                    { 6, "Belleville", 5, "NJ", "6 College St.", "07109" },
                    { 5, "Gastonia", 4, "NC", "7518 Sherwood Street", "28052" },
                    { 4, "Maplewood", 3, "NJ", "752 South Main Drive", "07040" },
                    { 2, "Green Bay", 2, "WI", "17 Johnson Street", "54302" },
                    { 19, "Saint Augustine", 19, "FL", "2 State St.", "32084" },
                    { 20, "Cedar Rapids", 20, "AZ", "8471 East Brandywine Street", "52402" }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CustomerId", "OrderDate", "StoreId" },
                values: new object[,]
                {
                    { 6, 5, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(9567), new TimeSpan(0, -7, 0, 0, 0)), 5 },
                    { 5, 4, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(9561), new TimeSpan(0, -7, 0, 0, 0)), 4 },
                    { 4, 3, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(9555), new TimeSpan(0, -7, 0, 0, 0)), 2 },
                    { 3, 2, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(9549), new TimeSpan(0, -7, 0, 0, 0)), 2 },
                    { 2, 1, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(9531), new TimeSpan(0, -7, 0, 0, 0)), 1 },
                    { 1, 1, new DateTimeOffset(new DateTime(2020, 5, 5, 6, 4, 6, 299, DateTimeKind.Unspecified).AddTicks(9242), new TimeSpan(0, -7, 0, 0, 0)), 1 }
                });

            migrationBuilder.InsertData(
                table: "OrderLineItems",
                columns: new[] { "Id", "InventoryItemId", "OrderId", "Price", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, 1, 150.55m, 2 },
                    { 2, 2, 1, 150.55m, 4 },
                    { 3, 3, 1, 150.55m, 5 },
                    { 4, 4, 1, 150.55m, 7 },
                    { 5, 1, 2, 150.55m, 3 },
                    { 6, 1, 3, 150.55m, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAddress_CustomerId",
                table: "CustomerAddress",
                column: "CustomerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_UserTypeId",
                table: "Customers",
                column: "UserTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_ProductId",
                table: "Inventory",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_StoreId",
                table: "Inventory",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLineItems_OrderId",
                table: "OrderLineItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_StoreId",
                table: "Orders",
                column: "StoreId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerAddress");

            migrationBuilder.DropTable(
                name: "Inventory");

            migrationBuilder.DropTable(
                name: "OrderLineItems");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Stores");

            migrationBuilder.DropTable(
                name: "UserTypes");
        }
    }
}
