using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Aryholding.Tms.TicketManagement.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModelSync : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TicketHistories_TicketId",
                table: "TicketHistories");

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PriorityLevels",
                keyColumn: "PriorityLevelId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PriorityLevels",
                keyColumn: "PriorityLevelId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PriorityLevels",
                keyColumn: "PriorityLevelId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PriorityLevels",
                keyColumn: "PriorityLevelId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "PriorityLevels",
                keyColumn: "PriorityLevelId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "PriorityLevels",
                keyColumn: "PriorityLevelId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "PriorityLevels",
                keyColumn: "PriorityLevelId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "PriorityLevels",
                keyColumn: "PriorityLevelId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "PriorityLevels",
                keyColumn: "PriorityLevelId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "PriorityLevels",
                keyColumn: "PriorityLevelId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "TicketStatuses",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.AlterColumn<string>(
                name: "eventMessage",
                table: "TicketHistories",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "NewStatusId",
                table: "TicketHistories",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OldStatusId",
                table: "TicketHistories",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TicketHistories_NewStatusId",
                table: "TicketHistories",
                column: "NewStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketHistories_OldStatusId",
                table: "TicketHistories",
                column: "OldStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketHistories_TicketId_ChangedAt",
                table: "TicketHistories",
                columns: new[] { "TicketId", "ChangedAt" });

            migrationBuilder.AddForeignKey(
                name: "FK_TicketHistories_TicketStatuses_NewStatusId",
                table: "TicketHistories",
                column: "NewStatusId",
                principalTable: "TicketStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketHistories_TicketStatuses_OldStatusId",
                table: "TicketHistories",
                column: "OldStatusId",
                principalTable: "TicketStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketHistories_TicketStatuses_NewStatusId",
                table: "TicketHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketHistories_TicketStatuses_OldStatusId",
                table: "TicketHistories");

            migrationBuilder.DropIndex(
                name: "IX_TicketHistories_NewStatusId",
                table: "TicketHistories");

            migrationBuilder.DropIndex(
                name: "IX_TicketHistories_OldStatusId",
                table: "TicketHistories");

            migrationBuilder.DropIndex(
                name: "IX_TicketHistories_TicketId_ChangedAt",
                table: "TicketHistories");

            migrationBuilder.DropColumn(
                name: "NewStatusId",
                table: "TicketHistories");

            migrationBuilder.DropColumn(
                name: "OldStatusId",
                table: "TicketHistories");

            migrationBuilder.AlterColumn<string>(
                name: "eventMessage",
                table: "TicketHistories",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "DepartmentId", "CreatedAt", "DepartmentCode", "DepartmentName", "IsActive", "UpdatedAt" },
                values: new object[] { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "IT", "Bilgi Teknolojileri", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "PriorityLevels",
                columns: new[] { "PriorityLevelId", "CreatedAt", "IsActive", "Level", "Name", "PriorityLevelCode", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 1, "Stajyer", "INTERN", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 2, "Personel", "STAFF", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 3, "Kıdemli Personel", "SENIOR_STAFF", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 4, "Uzman", "SPECIALIST", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 5, "Kıdemli Uzman", "SENIOR_SPECIALIST", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 6, "Ekip Lideri", "TEAM_LEAD", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 7, "Departman Yöneticisi", "DEPT_MANAGER", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 8, "Kıdemli Yönetici", "SENIOR_MANAGER", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 9, "Direktör", "DIRECTOR", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 10, "Departman Sahibi", "OWNER", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "TicketStatuses",
                columns: new[] { "Id", "Name", "TicketStatusCode" },
                values: new object[] { 5, "Reddedildi", "REJECTED" });

            migrationBuilder.CreateIndex(
                name: "IX_TicketHistories_TicketId",
                table: "TicketHistories",
                column: "TicketId");
        }
    }
}
