﻿// <auto-generated />
using System;
using Circle.Shared.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Circle.Shared.Migrations
{
    [DbContext(typeof(CircleDbContext))]
    partial class CircleDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Circle.Shared.Models.Businesses.Business", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Address2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Address3")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("BusinessType")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Verified")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Business");
                });

            modelBuilder.Entity("Circle.Shared.Models.Businesses.BusinessCategory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BusinessId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ParentBusinessCategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UniqueIdentiferCode")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BusinessId");

                    b.ToTable("BusinessCategory");
                });

            modelBuilder.Entity("Circle.Shared.Models.Businesses.BusinessListing", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BusinessCategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("DiscountPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("Discounted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Ratings")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int?>("Unit")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BusinessCategoryId");

                    b.ToTable("BusinessListing");
                });

            modelBuilder.Entity("Circle.Shared.Models.Businesses.BusinessListingDocument", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BusinessListingId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ContentType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("DocumentType")
                        .HasColumnType("int");

                    b.Property<long>("FileLength")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OriginalFileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Path")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BusinessListingId");

                    b.ToTable("BusinessListingDocument");
                });

            modelBuilder.Entity("Circle.Shared.Models.OpenIddict.CircleOpenIddictApplication", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AppId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClientId")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ClientSecret")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConcurrencyToken")
                        .IsConcurrencyToken()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ConsentType")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("DisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DisplayNames")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Language")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Permissions")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostLogoutRedirectUris")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Properties")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RedirectUris")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Requirements")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("ClientId")
                        .IsUnique()
                        .HasFilter("[ClientId] IS NOT NULL");

                    b.ToTable("OpenIddictApplications", (string)null);
                });

            modelBuilder.Entity("Circle.Shared.Models.OpenIddict.CircleOpenIddictAuthorization", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ApplicationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyToken")
                        .IsConcurrencyToken()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Properties")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Scopes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Subject")
                        .HasMaxLength(400)
                        .HasColumnType("nvarchar(400)");

                    b.Property<string>("Type")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId", "Status", "Subject", "Type");

                    b.ToTable("OpenIddictAuthorizations", (string)null);
                });

            modelBuilder.Entity("Circle.Shared.Models.OpenIddict.CircleOpenIddictScope", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyToken")
                        .IsConcurrencyToken()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Descriptions")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DisplayNames")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Properties")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Resources")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("OpenIddictScopes", (string)null);
                });

            modelBuilder.Entity("Circle.Shared.Models.OpenIddict.CircleOpenIddictToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ApplicationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AuthorizationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyToken")
                        .IsConcurrencyToken()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Payload")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Properties")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("RedemptionDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ReferenceId")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Status")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Subject")
                        .HasMaxLength(400)
                        .HasColumnType("nvarchar(400)");

                    b.Property<string>("Type")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("AuthorizationId");

                    b.HasIndex("ReferenceId")
                        .IsUnique()
                        .HasFilter("[ReferenceId] IS NOT NULL");

                    b.HasIndex("ApplicationId", "Status", "Subject", "Type");

                    b.ToTable("OpenIddictTokens", (string)null);
                });

            modelBuilder.Entity("Circle.Shared.Models.UserIdentity.AppRoleClaims", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("AppRoleClaims", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 5,
                            ClaimType = "Permission",
                            ClaimValue = "FULL_CONTROL",
                            RoleId = new Guid("773a3af2-cd9f-4f65-869f-0cfdc1e1589e")
                        },
                        new
                        {
                            Id = 6,
                            ClaimType = "Permission",
                            ClaimValue = "FULL_DEFAULT_USER_CONTROL",
                            RoleId = new Guid("cf185b00-652d-4c52-a3fb-4c94cb794718")
                        },
                        new
                        {
                            Id = 7,
                            ClaimType = "Permission",
                            ClaimValue = "FRONTDESK_CONTROL",
                            RoleId = new Guid("ca7061a2-138c-45b7-870c-699caa9ca99b")
                        },
                        new
                        {
                            Id = 8,
                            ClaimType = "Permission",
                            ClaimValue = "FULL_USER_CONTROL",
                            RoleId = new Guid("cc785f2a-2c0a-4648-87b7-a500084a2c1a")
                        });
                });

            modelBuilder.Entity("Circle.Shared.Models.UserIdentity.AppRoles", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("IsInBuilt")
                        .HasColumnType("bit");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AppRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("773a3af2-cd9f-4f65-869f-0cfdc1e1589e"),
                            ConcurrencyStamp = "79b7c6d29e404a959def18b75ca7b18a",
                            IsInBuilt = true,
                            Name = "SYS_ADMIN",
                            NormalizedName = "SYS_ADMIN"
                        },
                        new
                        {
                            Id = new Guid("cc785f2a-2c0a-4648-87b7-a500084a2c1a"),
                            ConcurrencyStamp = "0a9264319efa437a9083bce9fcdf97ee",
                            IsInBuilt = true,
                            Name = "ADMIN",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = new Guid("ca7061a2-138c-45b7-870c-699caa9ca99b"),
                            ConcurrencyStamp = "80aa50c42af1486da89a384945651dd0",
                            IsInBuilt = true,
                            Name = "FRONTDESK",
                            NormalizedName = "FRONTDESK"
                        },
                        new
                        {
                            Id = new Guid("cf185b00-652d-4c52-a3fb-4c94cb794718"),
                            ConcurrencyStamp = "903d3a51670e4719b78269ceb6a29154",
                            IsInBuilt = true,
                            Name = "DEFAULT",
                            NormalizedName = "DEFAULT"
                        });
                });

            modelBuilder.Entity("Circle.Shared.Models.UserIdentity.AppUserClaims", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("AppUserClaims", (string)null);
                });

            modelBuilder.Entity("Circle.Shared.Models.UserIdentity.AppUserLogins", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.ToTable("AppUserLogins", (string)null);
                });

            modelBuilder.Entity("Circle.Shared.Models.UserIdentity.AppUserRoles", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "RoleId");

                    b.ToTable("AppUserRoles", (string)null);

                    b.HasData(
                        new
                        {
                            UserId = new Guid("50b70c44-9eb7-4549-9a48-7d37809b7d8e"),
                            RoleId = new Guid("773a3af2-cd9f-4f65-869f-0cfdc1e1589e")
                        },
                        new
                        {
                            UserId = new Guid("1743b5bd-1eb1-45b3-9630-99596b17cf53"),
                            RoleId = new Guid("ca7061a2-138c-45b7-870c-699caa9ca99b")
                        },
                        new
                        {
                            UserId = new Guid("ca5eb7a4-de1e-40a1-9c58-ac452112aa92"),
                            RoleId = new Guid("cc785f2a-2c0a-4648-87b7-a500084a2c1a")
                        });
                });

            modelBuilder.Entity("Circle.Shared.Models.UserIdentity.AppUsers", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<bool>("Activated")
                        .HasColumnType("bit");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Department")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Gender")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsPasswordDefault")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastLoginDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("MiddleName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StaffNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("Unit")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserType")
                        .HasColumnType("int");

                    b.Property<Guid?>("UserTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("AppUsers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("50b70c44-9eb7-4549-9a48-7d37809b7d8e"),
                            AccessFailedCount = 0,
                            Activated = true,
                            ConcurrencyStamp = "78e4f500-6c6f-4b34-8e55-ce656f082fe6",
                            CreatedOn = new DateTime(2022, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "system@innercircle.com",
                            EmailConfirmed = true,
                            FirstName = "John",
                            IsDeleted = false,
                            LastLoginDate = new DateTime(2022, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Doe",
                            LockoutEnabled = false,
                            NormalizedEmail = "SYSTEM@INNERCIRCLE.COM",
                            NormalizedUserName = "SYSTEM@INNERCIRCLE.COM",
                            PasswordHash = "AQAAAAEAACcQAAAAEOAJi8ZBExzVsDrbwbYFINypUpgc9vTWFg3I6kez+dayxhmcGceXHAcd05SDiA1v1A==",
                            PhoneNumber = "08108565760",
                            PhoneNumberConfirmed = true,
                            SecurityStamp = "3c147856-b944-49f7-8c03-86eab5feadac",
                            TwoFactorEnabled = false,
                            UserName = "system@innercircle.com"
                        },
                        new
                        {
                            Id = new Guid("1743b5bd-1eb1-45b3-9630-99596b17cf53"),
                            AccessFailedCount = 0,
                            Activated = true,
                            ConcurrencyStamp = "45925e43-2da0-4406-85f9-5fc04931b8d9",
                            CreatedOn = new DateTime(2022, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "mohammedbello678@gmail.com",
                            EmailConfirmed = true,
                            FirstName = "Mohammed",
                            IsDeleted = false,
                            LastLoginDate = new DateTime(2022, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Bello",
                            LockoutEnabled = false,
                            NormalizedEmail = "MOHAMMEDBELLO678@GMAIL.COM",
                            NormalizedUserName = "MOHAMMEDBELLO678@GMAIL.COM",
                            PasswordHash = "AQAAAAEAACcQAAAAEJVQmfwiuKctp9UO46je1vOwtVn9P4ejz1HNdhuRdPAXw7l/1ZPHt0x9uWgjIQXqeQ==",
                            PhoneNumber = "09025055210",
                            PhoneNumberConfirmed = true,
                            SecurityStamp = "318338a4-8f26-47d7-bb01-66b8784aeae6",
                            TwoFactorEnabled = false,
                            UserName = "mohammedbello678@gmail.com"
                        },
                        new
                        {
                            Id = new Guid("ca5eb7a4-de1e-40a1-9c58-ac452112aa92"),
                            AccessFailedCount = 0,
                            Activated = true,
                            ConcurrencyStamp = "8b28b312-bcba-4d10-a325-691a147c4e5d",
                            CreatedOn = new DateTime(2022, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "admin@innercircle.com",
                            EmailConfirmed = true,
                            FirstName = "",
                            IsDeleted = false,
                            LastLoginDate = new DateTime(2022, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Admin",
                            LockoutEnabled = false,
                            NormalizedEmail = "ADMIN@INNERCIRCLE.COM",
                            NormalizedUserName = "ADMIN@INNERCIRCLE.COM",
                            PasswordHash = "AQAAAAEAACcQAAAAELxsUkPQZvhNXK4SPCp2YlC9m73aNN0eQN7uWO075l1JQzFXqSY5IZMYJSSbxwnbsQ==",
                            PhoneNumber = "09025055210",
                            PhoneNumberConfirmed = true,
                            SecurityStamp = "d2db0156-280e-4867-9795-8303362024dd",
                            TwoFactorEnabled = false,
                            UserName = "admin@innercircle.com"
                        },
                        new
                        {
                            Id = new Guid("96623538-0615-4d01-9023-7352bb4bb9c6"),
                            AccessFailedCount = 0,
                            Activated = true,
                            ConcurrencyStamp = "528ca840-8a9a-4645-af87-ce93cc0595f1",
                            CreatedOn = new DateTime(2020, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "frontdesk@innercircle.com",
                            EmailConfirmed = true,
                            FirstName = "babatunde",
                            IsDeleted = false,
                            LastLoginDate = new DateTime(2020, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Bello",
                            LockoutEnabled = false,
                            NormalizedEmail = "FRONTDESK@INNERCIRCLE.COM",
                            NormalizedUserName = "FRONTDESK@INNERCIRCLE.COM",
                            PasswordHash = "AQAAAAEAACcQAAAAEBCpyx60s5wN3f7uDn4rjy1S5pJTPwXFuDfwX328xCItZlw26Uxtyhq+mX7X5P2Qrw==",
                            PhoneNumber = "+2349025055210",
                            PhoneNumberConfirmed = true,
                            SecurityStamp = "81b94cda-96bb-43e0-ac86-6d4a3de474f9",
                            TwoFactorEnabled = false,
                            UserName = "frontdesk@innercircle.com"
                        });
                });

            modelBuilder.Entity("Circle.Shared.Models.UserIdentity.AppUserTokens", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AppUserTokens", (string)null);
                });

            modelBuilder.Entity("Circle.Shared.Models.Businesses.BusinessCategory", b =>
                {
                    b.HasOne("Circle.Shared.Models.Businesses.Business", "Business")
                        .WithMany()
                        .HasForeignKey("BusinessId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Business");
                });

            modelBuilder.Entity("Circle.Shared.Models.Businesses.BusinessListing", b =>
                {
                    b.HasOne("Circle.Shared.Models.Businesses.BusinessCategory", null)
                        .WithMany("BusinessListingDocuments")
                        .HasForeignKey("BusinessCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Circle.Shared.Models.Businesses.BusinessListingDocument", b =>
                {
                    b.HasOne("Circle.Shared.Models.Businesses.BusinessListing", null)
                        .WithMany("BusinessListingDocuments")
                        .HasForeignKey("BusinessListingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Circle.Shared.Models.OpenIddict.CircleOpenIddictAuthorization", b =>
                {
                    b.HasOne("Circle.Shared.Models.OpenIddict.CircleOpenIddictApplication", "Application")
                        .WithMany("Authorizations")
                        .HasForeignKey("ApplicationId");

                    b.Navigation("Application");
                });

            modelBuilder.Entity("Circle.Shared.Models.OpenIddict.CircleOpenIddictToken", b =>
                {
                    b.HasOne("Circle.Shared.Models.OpenIddict.CircleOpenIddictApplication", "Application")
                        .WithMany("Tokens")
                        .HasForeignKey("ApplicationId");

                    b.HasOne("Circle.Shared.Models.OpenIddict.CircleOpenIddictAuthorization", "Authorization")
                        .WithMany("Tokens")
                        .HasForeignKey("AuthorizationId");

                    b.Navigation("Application");

                    b.Navigation("Authorization");
                });

            modelBuilder.Entity("Circle.Shared.Models.Businesses.BusinessCategory", b =>
                {
                    b.Navigation("BusinessListingDocuments");
                });

            modelBuilder.Entity("Circle.Shared.Models.Businesses.BusinessListing", b =>
                {
                    b.Navigation("BusinessListingDocuments");
                });

            modelBuilder.Entity("Circle.Shared.Models.OpenIddict.CircleOpenIddictApplication", b =>
                {
                    b.Navigation("Authorizations");

                    b.Navigation("Tokens");
                });

            modelBuilder.Entity("Circle.Shared.Models.OpenIddict.CircleOpenIddictAuthorization", b =>
                {
                    b.Navigation("Tokens");
                });
#pragma warning restore 612, 618
        }
    }
}
