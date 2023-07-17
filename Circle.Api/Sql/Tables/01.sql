IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [AppRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] uniqueidentifier NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AppRoleClaims] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AppRoles] (
    [Id] uniqueidentifier NOT NULL,
    [CreatedOn] datetime2 NULL,
    [ModifiedOn] datetime2 NULL,
    [CreatedBy] uniqueidentifier NULL,
    [ModifiedBy] uniqueidentifier NULL,
    [IsInBuilt] bit NULL,
    [Name] nvarchar(max) NULL,
    [NormalizedName] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AppRoles] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AppUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] uniqueidentifier NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AppUserClaims] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AppUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [Id] int NOT NULL IDENTITY,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_AppUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey])
);
GO

CREATE TABLE [AppUserRoles] (
    [UserId] uniqueidentifier NOT NULL,
    [RoleId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_AppUserRoles] PRIMARY KEY ([UserId], [RoleId])
);
GO

CREATE TABLE [AppUsers] (
    [Id] uniqueidentifier NOT NULL,
    [RefreshToken] nvarchar(max) NULL,
    [ProviderKey] nvarchar(max) NULL,
    [LastName] nvarchar(max) NULL,
    [FirstName] nvarchar(max) NULL,
    [MiddleName] nvarchar(max) NULL,
    [Unit] nvarchar(max) NULL,
    [Gender] int NULL,
    [UserType] int NULL,
    [UserTypeId] uniqueidentifier NULL,
    [LastLoginDate] datetime2 NULL,
    [Activated] bit NOT NULL,
    [IsDeleted] bit NOT NULL,
    [CreatedOn] datetime2 NOT NULL,
    [ModifiedOn] datetime2 NULL,
    [CreatedBy] nvarchar(max) NULL,
    [ModifiedBy] nvarchar(max) NULL,
    [DeletedBy] nvarchar(max) NULL,
    [DeletedOn] datetime2 NULL,
    [Department] nvarchar(max) NULL,
    [IsPasswordDefault] bit NULL,
    [StaffNo] nvarchar(max) NULL,
    [UserName] nvarchar(max) NULL,
    [NormalizedUserName] nvarchar(max) NULL,
    [Email] nvarchar(max) NULL,
    [NormalizedEmail] nvarchar(max) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AppUsers] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AppUserTokens] (
    [UserId] uniqueidentifier NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AppUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name])
);
GO

CREATE TABLE [Business] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(max) NULL,
    [BusinessType] int NOT NULL,
    [Description] nvarchar(max) NULL,
    [Address1] nvarchar(max) NULL,
    [Address2] nvarchar(max) NULL,
    [Address3] nvarchar(max) NULL,
    [PhoneNumber1] nvarchar(max) NULL,
    [PhoneNumber2] nvarchar(max) NULL,
    [Email1] nvarchar(max) NULL,
    [Email2] nvarchar(max) NULL,
    [IsActive] bit NOT NULL,
    [Verified] bit NOT NULL,
    [CreatedOn] datetime2 NOT NULL,
    [ModifiedOn] datetime2 NULL,
    [CreatedBy] nvarchar(max) NULL,
    [ModifiedBy] nvarchar(max) NULL,
    [DeletedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedOn] datetime2 NULL,
    CONSTRAINT [PK_Business] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [OpenIddictApplications] (
    [Id] uniqueidentifier NOT NULL,
    [AppId] nvarchar(max) NULL,
    [Language] nvarchar(max) NULL,
    [ClientId] nvarchar(100) NULL,
    [ClientSecret] nvarchar(max) NULL,
    [ConcurrencyToken] nvarchar(50) NULL,
    [ConsentType] nvarchar(50) NULL,
    [DisplayName] nvarchar(max) NULL,
    [DisplayNames] nvarchar(max) NULL,
    [Permissions] nvarchar(max) NULL,
    [PostLogoutRedirectUris] nvarchar(max) NULL,
    [Properties] nvarchar(max) NULL,
    [RedirectUris] nvarchar(max) NULL,
    [Requirements] nvarchar(max) NULL,
    [Type] nvarchar(50) NULL,
    CONSTRAINT [PK_OpenIddictApplications] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [OpenIddictScopes] (
    [Id] uniqueidentifier NOT NULL,
    [ConcurrencyToken] nvarchar(50) NULL,
    [Description] nvarchar(max) NULL,
    [Descriptions] nvarchar(max) NULL,
    [DisplayName] nvarchar(max) NULL,
    [DisplayNames] nvarchar(max) NULL,
    [Name] nvarchar(200) NULL,
    [Properties] nvarchar(max) NULL,
    [Resources] nvarchar(max) NULL,
    CONSTRAINT [PK_OpenIddictScopes] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [UserProfile] (
    [Id] uniqueidentifier NOT NULL,
    [ProfileName] nvarchar(max) NULL,
    [Bio] nvarchar(max) NULL,
    [Location] nvarchar(max) NULL,
    [IsVerified] bit NULL,
    [ProfilePictureUrl] nvarchar(max) NULL,
    [BannerPictureUrl] nvarchar(max) NULL,
    [UserAccountId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_UserProfile] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UserProfile_AppUsers_UserAccountId] FOREIGN KEY ([UserAccountId]) REFERENCES [AppUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [BusinessCategory] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(max) NULL,
    [UniqueIdentiferCode] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [ParentBusinessCategoryId] uniqueidentifier NULL,
    [BusinessId] uniqueidentifier NOT NULL,
    [CreatedOn] datetime2 NOT NULL,
    [ModifiedOn] datetime2 NULL,
    [CreatedBy] nvarchar(max) NULL,
    [ModifiedBy] nvarchar(max) NULL,
    [DeletedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedOn] datetime2 NULL,
    CONSTRAINT [PK_BusinessCategory] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_BusinessCategory_Business_BusinessId] FOREIGN KEY ([BusinessId]) REFERENCES [Business] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [OpenIddictAuthorizations] (
    [Id] uniqueidentifier NOT NULL,
    [ApplicationId] uniqueidentifier NULL,
    [ConcurrencyToken] nvarchar(50) NULL,
    [CreationDate] datetime2 NULL,
    [Properties] nvarchar(max) NULL,
    [Scopes] nvarchar(max) NULL,
    [Status] nvarchar(50) NULL,
    [Subject] nvarchar(400) NULL,
    [Type] nvarchar(50) NULL,
    CONSTRAINT [PK_OpenIddictAuthorizations] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_OpenIddictAuthorizations_OpenIddictApplications_ApplicationId] FOREIGN KEY ([ApplicationId]) REFERENCES [OpenIddictApplications] ([Id])
);
GO

CREATE TABLE [BusinessListing] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Status] int NOT NULL,
    [Unit] int NULL,
    [Ratings] int NOT NULL,
    [Price] decimal(18,2) NOT NULL,
    [DiscountPrice] decimal(18,2) NOT NULL,
    [Discounted] bit NOT NULL,
    [BusinessCategoryId] uniqueidentifier NOT NULL,
    [CreatedOn] datetime2 NOT NULL,
    [ModifiedOn] datetime2 NULL,
    [CreatedBy] nvarchar(max) NULL,
    [ModifiedBy] nvarchar(max) NULL,
    [DeletedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedOn] datetime2 NULL,
    CONSTRAINT [PK_BusinessListing] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_BusinessListing_BusinessCategory_BusinessCategoryId] FOREIGN KEY ([BusinessCategoryId]) REFERENCES [BusinessCategory] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [OpenIddictTokens] (
    [Id] uniqueidentifier NOT NULL,
    [ApplicationId] uniqueidentifier NULL,
    [AuthorizationId] uniqueidentifier NULL,
    [ConcurrencyToken] nvarchar(50) NULL,
    [CreationDate] datetime2 NULL,
    [ExpirationDate] datetime2 NULL,
    [Payload] nvarchar(max) NULL,
    [Properties] nvarchar(max) NULL,
    [RedemptionDate] datetime2 NULL,
    [ReferenceId] nvarchar(100) NULL,
    [Status] nvarchar(50) NULL,
    [Subject] nvarchar(400) NULL,
    [Type] nvarchar(50) NULL,
    CONSTRAINT [PK_OpenIddictTokens] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_OpenIddictTokens_OpenIddictApplications_ApplicationId] FOREIGN KEY ([ApplicationId]) REFERENCES [OpenIddictApplications] ([Id]),
    CONSTRAINT [FK_OpenIddictTokens_OpenIddictAuthorizations_AuthorizationId] FOREIGN KEY ([AuthorizationId]) REFERENCES [OpenIddictAuthorizations] ([Id])
);
GO

CREATE TABLE [BusinessListingDocument] (
    [Id] uniqueidentifier NOT NULL,
    [Path] nvarchar(max) NULL,
    [ContentType] nvarchar(max) NULL,
    [FileLength] bigint NOT NULL,
    [OriginalFileName] nvarchar(max) NULL,
    [Name] nvarchar(max) NULL,
    [DocumentType] int NOT NULL,
    [BusinessListingId] uniqueidentifier NOT NULL,
    [CreatedOn] datetime2 NOT NULL,
    [ModifiedOn] datetime2 NULL,
    [CreatedBy] nvarchar(max) NULL,
    [ModifiedBy] nvarchar(max) NULL,
    [DeletedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedOn] datetime2 NULL,
    CONSTRAINT [PK_BusinessListingDocument] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_BusinessListingDocument_BusinessListing_BusinessListingId] FOREIGN KEY ([BusinessListingId]) REFERENCES [BusinessListing] ([Id]) ON DELETE CASCADE
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ClaimType', N'ClaimValue', N'RoleId') AND [object_id] = OBJECT_ID(N'[AppRoleClaims]'))
    SET IDENTITY_INSERT [AppRoleClaims] ON;
INSERT INTO [AppRoleClaims] ([Id], [ClaimType], [ClaimValue], [RoleId])
VALUES (5, N'Permission', N'FULL_CONTROL', '773a3af2-cd9f-4f65-869f-0cfdc1e1589e'),
(6, N'Permission', N'FULL_DEFAULT_USER_CONTROL', 'cf185b00-652d-4c52-a3fb-4c94cb794718'),
(7, N'Permission', N'FRONTDESK_CONTROL', 'ca7061a2-138c-45b7-870c-699caa9ca99b'),
(8, N'Permission', N'FULL_USER_CONTROL', 'cc785f2a-2c0a-4648-87b7-a500084a2c1a');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ClaimType', N'ClaimValue', N'RoleId') AND [object_id] = OBJECT_ID(N'[AppRoleClaims]'))
    SET IDENTITY_INSERT [AppRoleClaims] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'CreatedBy', N'CreatedOn', N'IsInBuilt', N'ModifiedBy', N'ModifiedOn', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AppRoles]'))
    SET IDENTITY_INSERT [AppRoles] ON;
INSERT INTO [AppRoles] ([Id], [ConcurrencyStamp], [CreatedBy], [CreatedOn], [IsInBuilt], [ModifiedBy], [ModifiedOn], [Name], [NormalizedName])
VALUES ('773a3af2-cd9f-4f65-869f-0cfdc1e1589e', N'178484085cc7404b922107e26afd9731', NULL, NULL, CAST(1 AS bit), NULL, NULL, N'SYS_ADMIN', N'SYS_ADMIN'),
('ca7061a2-138c-45b7-870c-699caa9ca99b', N'152f758736914f7fa10adc508f47fe7c', NULL, NULL, CAST(1 AS bit), NULL, NULL, N'FRONTDESK', N'FRONTDESK'),
('cc785f2a-2c0a-4648-87b7-a500084a2c1a', N'4d604a1de8fc4a3c8985af6e1905743b', NULL, NULL, CAST(1 AS bit), NULL, NULL, N'ADMIN', N'ADMIN'),
('cf185b00-652d-4c52-a3fb-4c94cb794718', N'a03e87f0d2e349a28b05fb6f2c4f1023', NULL, NULL, CAST(1 AS bit), NULL, NULL, N'DEFAULT', N'DEFAULT');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'CreatedBy', N'CreatedOn', N'IsInBuilt', N'ModifiedBy', N'ModifiedOn', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AppRoles]'))
    SET IDENTITY_INSERT [AppRoles] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'UserId') AND [object_id] = OBJECT_ID(N'[AppUserRoles]'))
    SET IDENTITY_INSERT [AppUserRoles] ON;
INSERT INTO [AppUserRoles] ([RoleId], [UserId])
VALUES ('ca7061a2-138c-45b7-870c-699caa9ca99b', '1743b5bd-1eb1-45b3-9630-99596b17cf53'),
('773a3af2-cd9f-4f65-869f-0cfdc1e1589e', '50b70c44-9eb7-4549-9a48-7d37809b7d8e'),
('cc785f2a-2c0a-4648-87b7-a500084a2c1a', 'ca5eb7a4-de1e-40a1-9c58-ac452112aa92');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'UserId') AND [object_id] = OBJECT_ID(N'[AppUserRoles]'))
    SET IDENTITY_INSERT [AppUserRoles] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AccessFailedCount', N'Activated', N'ConcurrencyStamp', N'CreatedBy', N'CreatedOn', N'DeletedBy', N'DeletedOn', N'Department', N'Email', N'EmailConfirmed', N'FirstName', N'Gender', N'IsDeleted', N'IsPasswordDefault', N'LastLoginDate', N'LastName', N'LockoutEnabled', N'LockoutEnd', N'MiddleName', N'ModifiedBy', N'ModifiedOn', N'NormalizedEmail', N'NormalizedUserName', N'PasswordHash', N'PhoneNumber', N'PhoneNumberConfirmed', N'ProviderKey', N'RefreshToken', N'SecurityStamp', N'StaffNo', N'TwoFactorEnabled', N'Unit', N'UserName', N'UserType', N'UserTypeId') AND [object_id] = OBJECT_ID(N'[AppUsers]'))
    SET IDENTITY_INSERT [AppUsers] ON;
INSERT INTO [AppUsers] ([Id], [AccessFailedCount], [Activated], [ConcurrencyStamp], [CreatedBy], [CreatedOn], [DeletedBy], [DeletedOn], [Department], [Email], [EmailConfirmed], [FirstName], [Gender], [IsDeleted], [IsPasswordDefault], [LastLoginDate], [LastName], [LockoutEnabled], [LockoutEnd], [MiddleName], [ModifiedBy], [ModifiedOn], [NormalizedEmail], [NormalizedUserName], [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed], [ProviderKey], [RefreshToken], [SecurityStamp], [StaffNo], [TwoFactorEnabled], [Unit], [UserName], [UserType], [UserTypeId])
VALUES ('1743b5bd-1eb1-45b3-9630-99596b17cf53', 0, CAST(1 AS bit), N'512ac03b-2bdc-4e35-9bb5-03a60aded6b1', NULL, '2022-10-15T00:00:00.0000000', NULL, NULL, NULL, N'mohammedbello678@gmail.com', CAST(1 AS bit), N'Mohammed', NULL, CAST(0 AS bit), NULL, '2022-10-15T00:00:00.0000000', N'Bello', CAST(0 AS bit), NULL, NULL, NULL, NULL, N'MOHAMMEDBELLO678@GMAIL.COM', N'MOHAMMEDBELLO678@GMAIL.COM', N'AQAAAAEAACcQAAAAEE8oNY0xHjCSRHAIfjy8JgyEqoq2EtWcQtZjGN9iEX4CIkyp3XxPLtq7XcLwIlBkLQ==', N'09025055210', CAST(1 AS bit), NULL, NULL, N'318338a4-8f26-47d7-bb01-66b8784aeae6', NULL, CAST(0 AS bit), NULL, N'mohammedbello678@gmail.com', NULL, NULL),
('50b70c44-9eb7-4549-9a48-7d37809b7d8e', 0, CAST(1 AS bit), N'94cb2ddf-240a-4213-9c6d-84b844f39021', NULL, '2022-10-15T00:00:00.0000000', NULL, NULL, NULL, N'system@innercircle.com', CAST(1 AS bit), N'John', NULL, CAST(0 AS bit), NULL, '2022-10-15T00:00:00.0000000', N'Doe', CAST(0 AS bit), NULL, NULL, NULL, NULL, N'SYSTEM@INNERCIRCLE.COM', N'SYSTEM@INNERCIRCLE.COM', N'AQAAAAEAACcQAAAAEAox/FAGYhGtHKLhMMsFdLAuFi7fNPRtyLhve7U5oAKNCIUQlS17AF0QQ+M4gdV8Fw==', N'08108565760', CAST(1 AS bit), NULL, NULL, N'3c147856-b944-49f7-8c03-86eab5feadac', NULL, CAST(0 AS bit), NULL, N'system@innercircle.com', NULL, NULL),
('96623538-0615-4d01-9023-7352bb4bb9c6', 0, CAST(1 AS bit), N'46a419e4-5c7b-4c20-a17a-eb9e96c3088a', NULL, '2020-10-15T00:00:00.0000000', NULL, NULL, NULL, N'frontdesk@innercircle.com', CAST(1 AS bit), N'babatunde', NULL, CAST(0 AS bit), NULL, '2020-10-15T00:00:00.0000000', N'Bello', CAST(0 AS bit), NULL, NULL, NULL, NULL, N'FRONTDESK@INNERCIRCLE.COM', N'FRONTDESK@INNERCIRCLE.COM', N'AQAAAAEAACcQAAAAEH/Qr89oS7k+FfMdieIXqFMvrGvnh+wpjvGFW6YMEtbyrRtVIeVd6hmC+l+aWBGdZA==', N'+2349025055210', CAST(1 AS bit), NULL, NULL, N'81b94cda-96bb-43e0-ac86-6d4a3de474f9', NULL, CAST(0 AS bit), NULL, N'frontdesk@innercircle.com', NULL, NULL),
('ca5eb7a4-de1e-40a1-9c58-ac452112aa92', 0, CAST(1 AS bit), N'2037a2cd-ddf2-499a-891b-864062bef2ee', NULL, '2022-10-15T00:00:00.0000000', NULL, NULL, NULL, N'admin@innercircle.com', CAST(1 AS bit), N'', NULL, CAST(0 AS bit), NULL, '2022-10-15T00:00:00.0000000', N'Admin', CAST(0 AS bit), NULL, NULL, NULL, NULL, N'ADMIN@INNERCIRCLE.COM', N'ADMIN@INNERCIRCLE.COM', N'AQAAAAEAACcQAAAAEKqXKw63GWFeoEHjjzeJFAt7jFB8CAAVRCrnloz64Fs16ijEIPWO0+ZpWABeBySNoA==', N'09025055210', CAST(1 AS bit), NULL, NULL, N'd2db0156-280e-4867-9795-8303362024dd', NULL, CAST(0 AS bit), NULL, N'admin@innercircle.com', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AccessFailedCount', N'Activated', N'ConcurrencyStamp', N'CreatedBy', N'CreatedOn', N'DeletedBy', N'DeletedOn', N'Department', N'Email', N'EmailConfirmed', N'FirstName', N'Gender', N'IsDeleted', N'IsPasswordDefault', N'LastLoginDate', N'LastName', N'LockoutEnabled', N'LockoutEnd', N'MiddleName', N'ModifiedBy', N'ModifiedOn', N'NormalizedEmail', N'NormalizedUserName', N'PasswordHash', N'PhoneNumber', N'PhoneNumberConfirmed', N'ProviderKey', N'RefreshToken', N'SecurityStamp', N'StaffNo', N'TwoFactorEnabled', N'Unit', N'UserName', N'UserType', N'UserTypeId') AND [object_id] = OBJECT_ID(N'[AppUsers]'))
    SET IDENTITY_INSERT [AppUsers] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'BannerPictureUrl', N'Bio', N'IsVerified', N'Location', N'ProfileName', N'ProfilePictureUrl', N'UserAccountId') AND [object_id] = OBJECT_ID(N'[UserProfile]'))
    SET IDENTITY_INSERT [UserProfile] ON;
INSERT INTO [UserProfile] ([Id], [BannerPictureUrl], [Bio], [IsVerified], [Location], [ProfileName], [ProfilePictureUrl], [UserAccountId])
VALUES ('3100f13f-25d8-4fd7-afde-1abd8a0d2354', N'https://twitter.com/Mohammed_kingin', N'The Circle management public profile', CAST(1 AS bit), N'Nigria', N'CIRCLE', N'https://twitter.com/Mohammed_kingin', '50b70c44-9eb7-4549-9a48-7d37809b7d8e');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'BannerPictureUrl', N'Bio', N'IsVerified', N'Location', N'ProfileName', N'ProfilePictureUrl', N'UserAccountId') AND [object_id] = OBJECT_ID(N'[UserProfile]'))
    SET IDENTITY_INSERT [UserProfile] OFF;
GO

CREATE INDEX [IX_BusinessCategory_BusinessId] ON [BusinessCategory] ([BusinessId]);
GO

CREATE INDEX [IX_BusinessListing_BusinessCategoryId] ON [BusinessListing] ([BusinessCategoryId]);
GO

CREATE INDEX [IX_BusinessListingDocument_BusinessListingId] ON [BusinessListingDocument] ([BusinessListingId]);
GO

CREATE UNIQUE INDEX [IX_OpenIddictApplications_ClientId] ON [OpenIddictApplications] ([ClientId]) WHERE [ClientId] IS NOT NULL;
GO

CREATE INDEX [IX_OpenIddictAuthorizations_ApplicationId_Status_Subject_Type] ON [OpenIddictAuthorizations] ([ApplicationId], [Status], [Subject], [Type]);
GO

CREATE UNIQUE INDEX [IX_OpenIddictScopes_Name] ON [OpenIddictScopes] ([Name]) WHERE [Name] IS NOT NULL;
GO

CREATE INDEX [IX_OpenIddictTokens_ApplicationId_Status_Subject_Type] ON [OpenIddictTokens] ([ApplicationId], [Status], [Subject], [Type]);
GO

CREATE INDEX [IX_OpenIddictTokens_AuthorizationId] ON [OpenIddictTokens] ([AuthorizationId]);
GO

CREATE UNIQUE INDEX [IX_OpenIddictTokens_ReferenceId] ON [OpenIddictTokens] ([ReferenceId]) WHERE [ReferenceId] IS NOT NULL;
GO

CREATE UNIQUE INDEX [IX_UserProfile_UserAccountId] ON [UserProfile] ([UserAccountId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230717114959_initial', N'6.0.12');
GO

COMMIT;
GO

