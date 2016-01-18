using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace Club.Migrations
{
    public partial class UserReferences : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_IdentityRoleClaim<string>_IdentityRole_RoleId", table: "AspNetRoleClaims");
            migrationBuilder.DropForeignKey(name: "FK_IdentityUserClaim<string>_ClubUser_UserId", table: "AspNetUserClaims");
            migrationBuilder.DropForeignKey(name: "FK_IdentityUserLogin<string>_ClubUser_UserId", table: "AspNetUserLogins");
            migrationBuilder.DropForeignKey(name: "FK_IdentityUserRole<string>_IdentityRole_RoleId", table: "AspNetUserRoles");
            migrationBuilder.DropForeignKey(name: "FK_IdentityUserRole<string>_ClubUser_UserId", table: "AspNetUserRoles");
            migrationBuilder.DropColumn(name: "Host", table: "Event");
            migrationBuilder.DropColumn(name: "CreatedDate", table: "Announcement");
            migrationBuilder.DropColumn(name: "Creator", table: "Announcement");
            migrationBuilder.AddColumn<string>(
                name: "ClubUserHostId",
                table: "Event",
                nullable: true);
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Event",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
            migrationBuilder.AddColumn<string>(
                name: "EventCode",
                table: "Event",
                nullable: true);
            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Event",
                nullable: true);
            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                nullable: true);
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                nullable: true);
            migrationBuilder.AddColumn<string>(
                name: "ClubUserCreatorId",
                table: "Announcement",
                nullable: true);
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Announcement",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "Announcement",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "Announcement",
                nullable: true);
            migrationBuilder.AddForeignKey(
                name: "FK_Announcement_ClubUser_ClubUserCreatorId",
                table: "Announcement",
                column: "ClubUserCreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_Event_ClubUser_ClubUserHostId",
                table: "Event",
                column: "ClubUserHostId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityRoleClaim<string>_IdentityRole_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserClaim<string>_ClubUser_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserLogin<string>_ClubUser_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserRole<string>_IdentityRole_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserRole<string>_ClubUser_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Announcement_ClubUser_ClubUserCreatorId", table: "Announcement");
            migrationBuilder.DropForeignKey(name: "FK_Event_ClubUser_ClubUserHostId", table: "Event");
            migrationBuilder.DropForeignKey(name: "FK_IdentityRoleClaim<string>_IdentityRole_RoleId", table: "AspNetRoleClaims");
            migrationBuilder.DropForeignKey(name: "FK_IdentityUserClaim<string>_ClubUser_UserId", table: "AspNetUserClaims");
            migrationBuilder.DropForeignKey(name: "FK_IdentityUserLogin<string>_ClubUser_UserId", table: "AspNetUserLogins");
            migrationBuilder.DropForeignKey(name: "FK_IdentityUserRole<string>_IdentityRole_RoleId", table: "AspNetUserRoles");
            migrationBuilder.DropForeignKey(name: "FK_IdentityUserRole<string>_ClubUser_UserId", table: "AspNetUserRoles");
            migrationBuilder.DropColumn(name: "ClubUserHostId", table: "Event");
            migrationBuilder.DropColumn(name: "CreatedOn", table: "Event");
            migrationBuilder.DropColumn(name: "EventCode", table: "Event");
            migrationBuilder.DropColumn(name: "Location", table: "Event");
            migrationBuilder.DropColumn(name: "Approved", table: "AspNetUsers");
            migrationBuilder.DropColumn(name: "FirstName", table: "AspNetUsers");
            migrationBuilder.DropColumn(name: "IsActive", table: "AspNetUsers");
            migrationBuilder.DropColumn(name: "LastName", table: "AspNetUsers");
            migrationBuilder.DropColumn(name: "ClubUserCreatorId", table: "Announcement");
            migrationBuilder.DropColumn(name: "CreatedOn", table: "Announcement");
            migrationBuilder.DropColumn(name: "DueDate", table: "Announcement");
            migrationBuilder.DropColumn(name: "Text", table: "Announcement");
            migrationBuilder.AddColumn<string>(
                name: "Host",
                table: "Event",
                nullable: true);
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Announcement",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "Announcement",
                nullable: true);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityRoleClaim<string>_IdentityRole_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserClaim<string>_ClubUser_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserLogin<string>_ClubUser_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserRole<string>_IdentityRole_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserRole<string>_ClubUser_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
