﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Circle.Shared.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsInBuilt = table.Column<bool>(type: "bit", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProviderKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: true),
                    UserType = table.Column<int>(type: "int", nullable: true),
                    UserTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastLoginDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Activated = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPasswordDefault = table.Column<bool>(type: "bit", nullable: true),
                    StaffNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Business",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BusinessType = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Verified = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Business", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OpenIddictApplications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ClientSecret = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyToken = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ConsentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Permissions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostLogoutRedirectUris = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Properties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RedirectUris = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Requirements = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenIddictApplications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OpenIddictScopes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConcurrencyToken = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descriptions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Properties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Resources = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenIddictScopes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppRoleClaims_AppRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AppRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUserClaims_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AppUserLogins_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AppUserRoles_AppRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AppRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserRoles_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUserTokenMap",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserTokenMap", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_AppUserTokenMap_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BusinessCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UniqueIdentiferCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentBusinessCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BusinessId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessCategory_Business_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Business",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OpenIddictAuthorizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ConcurrencyToken = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Properties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Scopes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenIddictAuthorizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenIddictAuthorizations_OpenIddictApplications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "OpenIddictApplications",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BusinessListing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Unit = table.Column<int>(type: "int", nullable: true),
                    Ratings = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discounted = table.Column<bool>(type: "bit", nullable: false),
                    BusinessCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessListing", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessListing_BusinessCategory_BusinessCategoryId",
                        column: x => x.BusinessCategoryId,
                        principalTable: "BusinessCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OpenIddictTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AuthorizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ConcurrencyToken = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Payload = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Properties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RedemptionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReferenceId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenIddictTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenIddictTokens_OpenIddictApplications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "OpenIddictApplications",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OpenIddictTokens_OpenIddictAuthorizations_AuthorizationId",
                        column: x => x.AuthorizationId,
                        principalTable: "OpenIddictAuthorizations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BusinessListingDocument",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileLength = table.Column<long>(type: "bigint", nullable: false),
                    OriginalFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentType = table.Column<int>(type: "int", nullable: false),
                    BusinessListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessListingDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessListingDocument_BusinessListing_BusinessListingId",
                        column: x => x.BusinessListingId,
                        principalTable: "BusinessListing",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AppRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "CreatedBy", "CreatedOn", "IsInBuilt", "ModifiedBy", "ModifiedOn", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("773a3af2-cd9f-4f65-869f-0cfdc1e1589e"), "ae6c1d1b61084320b8b05d4fc4fdd698", null, null, true, null, null, "SYS_ADMIN", "SYS_ADMIN" },
                    { new Guid("ca7061a2-138c-45b7-870c-699caa9ca99b"), "de3b7e90e1ed43dfb471b3ce27fc6dad", null, null, true, null, null, "FRONTDESK", "FRONTDESK" },
                    { new Guid("cc785f2a-2c0a-4648-87b7-a500084a2c1a"), "61a5fc822c284b27932019efdc43eb67", null, null, true, null, null, "ADMIN", "ADMIN" },
                    { new Guid("cf185b00-652d-4c52-a3fb-4c94cb794718"), "5c315cf433e94597911491552ff9c1bf", null, null, true, null, null, "DEFAULT", "DEFAULT" }
                });

            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "Id", "AccessFailedCount", "Activated", "ConcurrencyStamp", "CreatedBy", "CreatedOn", "DeletedBy", "DeletedOn", "Department", "Email", "EmailConfirmed", "FirstName", "Gender", "IsDeleted", "IsPasswordDefault", "LastLoginDate", "LastName", "LockoutEnabled", "LockoutEnd", "MiddleName", "ModifiedBy", "ModifiedOn", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProviderKey", "RefreshToken", "SecurityStamp", "StaffNo", "TwoFactorEnabled", "Unit", "UserName", "UserType", "UserTypeId" },
                values: new object[,]
                {
                    { new Guid("1743b5bd-1eb1-45b3-9630-99596b17cf53"), 0, true, "045a66de-fbd0-4071-ae69-bfae666fcdf4", null, new DateTime(2022, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "mohammedbello678@gmail.com", true, "Mohammed", null, false, null, new DateTime(2022, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bello", false, null, null, null, null, "MOHAMMEDBELLO678@GMAIL.COM", "MOHAMMEDBELLO678@GMAIL.COM", "AQAAAAEAACcQAAAAEFe8RUt6303DWqoHlFJoifcDIV4xUm21JmoY852UZJkUhK3dvJnEPlDeRO98cXL6KA==", "09025055210", true, null, null, "318338a4-8f26-47d7-bb01-66b8784aeae6", null, false, null, "mohammedbello678@gmail.com", null, null },
                    { new Guid("50b70c44-9eb7-4549-9a48-7d37809b7d8e"), 0, true, "9e03cd83-710e-4cf1-93fd-a1755114432d", null, new DateTime(2022, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "system@innercircle.com", true, "John", null, false, null, new DateTime(2022, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Doe", false, null, null, null, null, "SYSTEM@INNERCIRCLE.COM", "SYSTEM@INNERCIRCLE.COM", "AQAAAAEAACcQAAAAEPbsX8DcqTiE3nm/tC6Mf2Henep0d8D4Y2uQ4w4wx8FdVilq05FWoGAwpM13bVOW8A==", "08108565760", true, null, null, "3c147856-b944-49f7-8c03-86eab5feadac", null, false, null, "system@innercircle.com", null, null },
                    { new Guid("96623538-0615-4d01-9023-7352bb4bb9c6"), 0, true, "459294c8-d398-40b3-9b9c-55c0ac8f4c48", null, new DateTime(2020, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "frontdesk@innercircle.com", true, "babatunde", null, false, null, new DateTime(2020, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bello", false, null, null, null, null, "FRONTDESK@INNERCIRCLE.COM", "FRONTDESK@INNERCIRCLE.COM", "AQAAAAEAACcQAAAAEOm4uCPyVt3TKoZTiPxBmZLmDwDfMNGAUcdRnjlC+few4MCcaIUODMCVwaHRxoJx/A==", "+2349025055210", true, null, null, "81b94cda-96bb-43e0-ac86-6d4a3de474f9", null, false, null, "frontdesk@innercircle.com", null, null },
                    { new Guid("ca5eb7a4-de1e-40a1-9c58-ac452112aa92"), 0, true, "d6b62f0f-eec1-437a-9faf-0c93b254fd37", null, new DateTime(2022, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "admin@innercircle.com", true, "", null, false, null, new DateTime(2022, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", false, null, null, null, null, "ADMIN@INNERCIRCLE.COM", "ADMIN@INNERCIRCLE.COM", "AQAAAAEAACcQAAAAEBGk1G9TSAb/LxllQr9/h2PrR8cHtHu+JKnN42LTbDFTe1ZtRiEq4+/mZjYfVaizww==", "09025055210", true, null, null, "d2db0156-280e-4867-9795-8303362024dd", null, false, null, "admin@innercircle.com", null, null }
                });

            migrationBuilder.InsertData(
                table: "AppUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("ca7061a2-138c-45b7-870c-699caa9ca99b"), new Guid("1743b5bd-1eb1-45b3-9630-99596b17cf53") });

            migrationBuilder.InsertData(
                table: "AppUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("773a3af2-cd9f-4f65-869f-0cfdc1e1589e"), new Guid("50b70c44-9eb7-4549-9a48-7d37809b7d8e") });

            migrationBuilder.InsertData(
                table: "AppUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("cc785f2a-2c0a-4648-87b7-a500084a2c1a"), new Guid("ca5eb7a4-de1e-40a1-9c58-ac452112aa92") });

            migrationBuilder.CreateIndex(
                name: "IX_AppRoleClaims_RoleId",
                table: "AppRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AppRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserClaims_UserId",
                table: "AppUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserLogins_UserId",
                table: "AppUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserRoles_RoleId",
                table: "AppUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AppUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AppUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessCategory_BusinessId",
                table: "BusinessCategory",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessListing_BusinessCategoryId",
                table: "BusinessListing",
                column: "BusinessCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessListingDocument_BusinessListingId",
                table: "BusinessListingDocument",
                column: "BusinessListingId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictApplications_ClientId",
                table: "OpenIddictApplications",
                column: "ClientId",
                unique: true,
                filter: "[ClientId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictAuthorizations_ApplicationId_Status_Subject_Type",
                table: "OpenIddictAuthorizations",
                columns: new[] { "ApplicationId", "Status", "Subject", "Type" });

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictScopes_Name",
                table: "OpenIddictScopes",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictTokens_ApplicationId_Status_Subject_Type",
                table: "OpenIddictTokens",
                columns: new[] { "ApplicationId", "Status", "Subject", "Type" });

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictTokens_AuthorizationId",
                table: "OpenIddictTokens",
                column: "AuthorizationId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictTokens_ReferenceId",
                table: "OpenIddictTokens",
                column: "ReferenceId",
                unique: true,
                filter: "[ReferenceId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppRoleClaims");

            migrationBuilder.DropTable(
                name: "AppUserClaims");

            migrationBuilder.DropTable(
                name: "AppUserLogins");

            migrationBuilder.DropTable(
                name: "AppUserRoles");

            migrationBuilder.DropTable(
                name: "AppUserTokenMap");

            migrationBuilder.DropTable(
                name: "BusinessListingDocument");

            migrationBuilder.DropTable(
                name: "OpenIddictScopes");

            migrationBuilder.DropTable(
                name: "OpenIddictTokens");

            migrationBuilder.DropTable(
                name: "AppRoles");

            migrationBuilder.DropTable(
                name: "AppUsers");

            migrationBuilder.DropTable(
                name: "BusinessListing");

            migrationBuilder.DropTable(
                name: "OpenIddictAuthorizations");

            migrationBuilder.DropTable(
                name: "BusinessCategory");

            migrationBuilder.DropTable(
                name: "OpenIddictApplications");

            migrationBuilder.DropTable(
                name: "Business");
        }
    }
}
