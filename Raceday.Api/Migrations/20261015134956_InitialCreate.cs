using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raceday.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Club",
                columns: table => new
                {
                    clubID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "15432, 1"),
                    club_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    province = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    city = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    contact_email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    sport_type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Club", x => x.clubID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    userID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1401, 1"),
                    fullname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    surname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    user_password = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    phone_number = table.Column<int>(type: "int", nullable: false),
                    user_role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    clubID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.userID);
                    table.ForeignKey(
                        name: "FK_Users_Club_clubID",
                        column: x => x.clubID,
                        principalTable: "Club",
                        principalColumn: "clubID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    eventID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 10"),
                    organiserID = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    event_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    event_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    registration_deadline = table.Column<DateTime>(type: "datetime2", nullable: false),
                    venue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    city = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    province = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.eventID);
                    table.ForeignKey(
                        name: "FK_Event_Users_organiserID",
                        column: x => x.organiserID,
                        principalTable: "Users",
                        principalColumn: "userID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    categoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "101, 100"),
                    eventID = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    distance_KM = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    start_time = table.Column<TimeSpan>(type: "time", nullable: false),
                    entry_fee = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    age_category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    max_entries = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.categoryID);
                    table.ForeignKey(
                        name: "FK_Category_Event_eventID",
                        column: x => x.eventID,
                        principalTable: "Event",
                        principalColumn: "eventID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Enrolment",
                columns: table => new
                {
                    enrolmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "20058, 20"),
                    participantID = table.Column<int>(type: "int", nullable: false),
                    categoryID = table.Column<int>(type: "int", nullable: false),
                    racer_number = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    enrolment_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    payment_status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrolment", x => x.enrolmentID);
                    table.ForeignKey(
                        name: "FK_Enrolment_Category_categoryID",
                        column: x => x.categoryID,
                        principalTable: "Category",
                        principalColumn: "categoryID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enrolment_Users_participantID",
                        column: x => x.participantID,
                        principalTable: "Users",
                        principalColumn: "userID");
                });

            migrationBuilder.CreateTable(
                name: "Result",
                columns: table => new
                {
                    resultID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    enrolmentID = table.Column<int>(type: "int", nullable: false),
                    finish_time = table.Column<TimeSpan>(type: "time", nullable: true),
                    category_position = table.Column<int>(type: "int", nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Result", x => x.resultID);
                    table.ForeignKey(
                        name: "FK_Result_Enrolment_enrolmentID",
                        column: x => x.enrolmentID,
                        principalTable: "Enrolment",
                        principalColumn: "enrolmentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Category_eventID",
                table: "Category",
                column: "eventID");

            migrationBuilder.CreateIndex(
                name: "IX_Club_club_name",
                table: "Club",
                column: "club_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Enrolment_categoryID",
                table: "Enrolment",
                column: "categoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Enrolment_participantID",
                table: "Enrolment",
                column: "participantID");

            migrationBuilder.CreateIndex(
                name: "IX_Event_organiserID",
                table: "Event",
                column: "organiserID");

            migrationBuilder.CreateIndex(
                name: "IX_Result_enrolmentID",
                table: "Result",
                column: "enrolmentID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_clubID",
                table: "Users",
                column: "clubID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_email",
                table: "Users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_phone_number",
                table: "Users",
                column: "phone_number",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Result");

            migrationBuilder.DropTable(
                name: "Enrolment");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Club");
        }
    }
}
