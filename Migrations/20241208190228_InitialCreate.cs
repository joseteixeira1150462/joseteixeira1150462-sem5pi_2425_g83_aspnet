using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthCare.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OperationRequest",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PatientId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoctorId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OperationTypeId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeadlineDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationRequest", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OperationTypes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PasswordHash_Hash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email_Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    LoginAttempts = table.Column<int>(type: "int", nullable: false),
                    LastRequest = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OperationTypeVersion",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Starting = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ending = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Duration_Preparation = table.Column<int>(type: "int", nullable: true),
                    Duration_Operation = table.Column<int>(type: "int", nullable: true),
                    Duration_Cleaning = table.Column<int>(type: "int", nullable: true),
                    OperationTypeId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationTypeVersion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperationTypeVersion_OperationTypes_OperationTypeId",
                        column: x => x.OperationTypeId,
                        principalTable: "OperationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(101)", maxLength: 101, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MedicalRecordNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    HealthConditions = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    EmergencyPhone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    SequentialNumber = table.Column<int>(type: "int", nullable: false),
                    UpdatesExecuted = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Patients_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Staffs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LicenseNumber_Number = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Specialization = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone_Value = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: true),
                    Email_Value = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SequencialNumber = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staffs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Staffs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OperationTypeSpecialization",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OperationTypeVersionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    Specialization = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationTypeSpecialization", x => new { x.OperationTypeVersionId, x.Id });
                    table.ForeignKey(
                        name: "FK_OperationTypeSpecialization_OperationTypeVersion_OperationTypeVersionId",
                        column: x => x.OperationTypeVersionId,
                        principalTable: "OperationTypeVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TimeSlot",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StaffId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Starting = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ending = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeSlot", x => new { x.StaffId, x.Id });
                    table.ForeignKey(
                        name: "FK_TimeSlot_Staffs_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OperationTypeVersion_OperationTypeId",
                table: "OperationTypeVersion",
                column: "OperationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_EmailAddress",
                table: "Patients",
                column: "EmailAddress",
                unique: true,
                filter: "[EmailAddress] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_MedicalRecordNumber",
                table: "Patients",
                column: "MedicalRecordNumber",
                unique: true,
                filter: "[MedicalRecordNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_Phone",
                table: "Patients",
                column: "Phone",
                unique: true,
                filter: "[Phone] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_UserId",
                table: "Patients",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Staffs_Email_Value",
                table: "Staffs",
                column: "Email_Value",
                unique: true,
                filter: "[Email_Value] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Staffs_LicenseNumber_Number",
                table: "Staffs",
                column: "LicenseNumber_Number",
                unique: true,
                filter: "[LicenseNumber_Number] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Staffs_Phone_Value",
                table: "Staffs",
                column: "Phone_Value",
                unique: true,
                filter: "[Phone_Value] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Staffs_UserId",
                table: "Staffs",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OperationRequest");

            migrationBuilder.DropTable(
                name: "OperationTypeSpecialization");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "TimeSlot");

            migrationBuilder.DropTable(
                name: "OperationTypeVersion");

            migrationBuilder.DropTable(
                name: "Staffs");

            migrationBuilder.DropTable(
                name: "OperationTypes");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
