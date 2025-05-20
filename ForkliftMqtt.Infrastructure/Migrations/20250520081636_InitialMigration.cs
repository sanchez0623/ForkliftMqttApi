using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ForkliftMqtt.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Forklifts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastMaintenanceDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forklifts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SensorReadings",
                columns: table => new
                {
                    SensorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Value = table.Column<double>(type: "float", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Attributes = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorReadings", x => new { x.SensorId, x.Timestamp });
                });

            migrationBuilder.CreateTable(
                name: "Sensors",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ForkliftId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    ForkliftId1 = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sensors_Forklifts_ForkliftId",
                        column: x => x.ForkliftId,
                        principalTable: "Forklifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sensors_Forklifts_ForkliftId1",
                        column: x => x.ForkliftId1,
                        principalTable: "Forklifts",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Forklifts",
                columns: new[] { "Id", "LastMaintenanceDate", "Model", "Name", "Status" },
                values: new object[,]
                {
                    { "FL-001", new DateTime(2025, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "XC-2023", "叉车一号", "Idle" },
                    { "FL-002", new DateTime(2025, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "XC-2023", "叉车二号", "Working" }
                });

            migrationBuilder.InsertData(
                table: "Sensors",
                columns: new[] { "Id", "ForkliftId", "ForkliftId1", "Location", "Metadata", "Type" },
                values: new object[,]
                {
                    { "SENS-001", "FL-001", null, "前部", "{\"range\":\"0-100\",\"alertThreshold\":80}", "Temperature" },
                    { "SENS-002", "FL-001", null, "中部", "{\"range\":\"0-100\",\"alertThreshold\":90}", "Humidity" },
                    { "SENS-003", "FL-002", null, "电池仓", "{\"capacity\":\"200Ah\",\"alertThreshold\":20}", "Battery" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_SensorReadings_SensorId",
                table: "SensorReadings",
                column: "SensorId");

            migrationBuilder.CreateIndex(
                name: "IX_SensorReadings_Timestamp",
                table: "SensorReadings",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_ForkliftId",
                table: "Sensors",
                column: "ForkliftId");

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_ForkliftId1",
                table: "Sensors",
                column: "ForkliftId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SensorReadings");

            migrationBuilder.DropTable(
                name: "Sensors");

            migrationBuilder.DropTable(
                name: "Forklifts");
        }
    }
}
