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

CREATE TABLE [AppRoles] (
    [Id] uniqueidentifier NOT NULL,
    [CreatedOn] datetime2 NULL,
    [ModifiedOn] datetime2 NULL,
    [CreatedBy] uniqueidentifier NULL,
    [ModifiedBy] uniqueidentifier NULL,
    [IsInBuilt] bit NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AppRoles] PRIMARY KEY ([Id])
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
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
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

CREATE TABLE [AppRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] uniqueidentifier NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AppRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AppRoleClaims_AppRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AppRoles] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AppUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] uniqueidentifier NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AppUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AppUserClaims_AppUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AppUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AppUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [Id] int NOT NULL IDENTITY,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_AppUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AppUserLogins_AppUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AppUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AppUserRoles] (
    [UserId] uniqueidentifier NOT NULL,
    [RoleId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_AppUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AppUserRoles_AppRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AppRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AppUserRoles_AppUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AppUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AppUserTokenMap] (
    [UserId] uniqueidentifier NOT NULL,
    [LoginProvider] nvarchar(max) NULL,
    [Name] nvarchar(max) NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AppUserTokenMap] PRIMARY KEY ([UserId]),
    CONSTRAINT [FK_AppUserTokenMap_AppUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AppUsers] ([Id]) ON DELETE CASCADE
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

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'CreatedBy', N'CreatedOn', N'IsInBuilt', N'ModifiedBy', N'ModifiedOn', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AppRoles]'))
    SET IDENTITY_INSERT [AppRoles] ON;
INSERT INTO [AppRoles] ([Id], [ConcurrencyStamp], [CreatedBy], [CreatedOn], [IsInBuilt], [ModifiedBy], [ModifiedOn], [Name], [NormalizedName])
VALUES ('773a3af2-cd9f-4f65-869f-0cfdc1e1589e', N'ae6c1d1b61084320b8b05d4fc4fdd698', NULL, NULL, CAST(1 AS bit), NULL, NULL, N'SYS_ADMIN', N'SYS_ADMIN'),
('ca7061a2-138c-45b7-870c-699caa9ca99b', N'de3b7e90e1ed43dfb471b3ce27fc6dad', NULL, NULL, CAST(1 AS bit), NULL, NULL, N'FRONTDESK', N'FRONTDESK'),
('cc785f2a-2c0a-4648-87b7-a500084a2c1a', N'61a5fc822c284b27932019efdc43eb67', NULL, NULL, CAST(1 AS bit), NULL, NULL, N'ADMIN', N'ADMIN'),
('cf185b00-652d-4c52-a3fb-4c94cb794718', N'5c315cf433e94597911491552ff9c1bf', NULL, NULL, CAST(1 AS bit), NULL, NULL, N'DEFAULT', N'DEFAULT');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'CreatedBy', N'CreatedOn', N'IsInBuilt', N'ModifiedBy', N'ModifiedOn', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AppRoles]'))
    SET IDENTITY_INSERT [AppRoles] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AccessFailedCount', N'Activated', N'ConcurrencyStamp', N'CreatedBy', N'CreatedOn', N'DeletedBy', N'DeletedOn', N'Department', N'Email', N'EmailConfirmed', N'FirstName', N'Gender', N'IsDeleted', N'IsPasswordDefault', N'LastLoginDate', N'LastName', N'LockoutEnabled', N'LockoutEnd', N'MiddleName', N'ModifiedBy', N'ModifiedOn', N'NormalizedEmail', N'NormalizedUserName', N'PasswordHash', N'PhoneNumber', N'PhoneNumberConfirmed', N'ProviderKey', N'RefreshToken', N'SecurityStamp', N'StaffNo', N'TwoFactorEnabled', N'Unit', N'UserName', N'UserType', N'UserTypeId') AND [object_id] = OBJECT_ID(N'[AppUsers]'))
    SET IDENTITY_INSERT [AppUsers] ON;
INSERT INTO [AppUsers] ([Id], [AccessFailedCount], [Activated], [ConcurrencyStamp], [CreatedBy], [CreatedOn], [DeletedBy], [DeletedOn], [Department], [Email], [EmailConfirmed], [FirstName], [Gender], [IsDeleted], [IsPasswordDefault], [LastLoginDate], [LastName], [LockoutEnabled], [LockoutEnd], [MiddleName], [ModifiedBy], [ModifiedOn], [NormalizedEmail], [NormalizedUserName], [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed], [ProviderKey], [RefreshToken], [SecurityStamp], [StaffNo], [TwoFactorEnabled], [Unit], [UserName], [UserType], [UserTypeId])
VALUES ('1743b5bd-1eb1-45b3-9630-99596b17cf53', 0, CAST(1 AS bit), N'045a66de-fbd0-4071-ae69-bfae666fcdf4', NULL, '2022-10-15T00:00:00.0000000', NULL, NULL, NULL, N'mohammedbello678@gmail.com', CAST(1 AS bit), N'Mohammed', NULL, CAST(0 AS bit), NULL, '2022-10-15T00:00:00.0000000', N'Bello', CAST(0 AS bit), NULL, NULL, NULL, NULL, N'MOHAMMEDBELLO678@GMAIL.COM', N'MOHAMMEDBELLO678@GMAIL.COM', N'AQAAAAEAACcQAAAAEFe8RUt6303DWqoHlFJoifcDIV4xUm21JmoY852UZJkUhK3dvJnEPlDeRO98cXL6KA==', N'09025055210', CAST(1 AS bit), NULL, NULL, N'318338a4-8f26-47d7-bb01-66b8784aeae6', NULL, CAST(0 AS bit), NULL, N'mohammedbello678@gmail.com', NULL, NULL),
('50b70c44-9eb7-4549-9a48-7d37809b7d8e', 0, CAST(1 AS bit), N'9e03cd83-710e-4cf1-93fd-a1755114432d', NULL, '2022-10-15T00:00:00.0000000', NULL, NULL, NULL, N'system@innercircle.com', CAST(1 AS bit), N'John', NULL, CAST(0 AS bit), NULL, '2022-10-15T00:00:00.0000000', N'Doe', CAST(0 AS bit), NULL, NULL, NULL, NULL, N'SYSTEM@INNERCIRCLE.COM', N'SYSTEM@INNERCIRCLE.COM', N'AQAAAAEAACcQAAAAEPbsX8DcqTiE3nm/tC6Mf2Henep0d8D4Y2uQ4w4wx8FdVilq05FWoGAwpM13bVOW8A==', N'08108565760', CAST(1 AS bit), NULL, NULL, N'3c147856-b944-49f7-8c03-86eab5feadac', NULL, CAST(0 AS bit), NULL, N'system@innercircle.com', NULL, NULL),
('96623538-0615-4d01-9023-7352bb4bb9c6', 0, CAST(1 AS bit), N'459294c8-d398-40b3-9b9c-55c0ac8f4c48', NULL, '2020-10-15T00:00:00.0000000', NULL, NULL, NULL, N'frontdesk@innercircle.com', CAST(1 AS bit), N'babatunde', NULL, CAST(0 AS bit), NULL, '2020-10-15T00:00:00.0000000', N'Bello', CAST(0 AS bit), NULL, NULL, NULL, NULL, N'FRONTDESK@INNERCIRCLE.COM', N'FRONTDESK@INNERCIRCLE.COM', N'AQAAAAEAACcQAAAAEOm4uCPyVt3TKoZTiPxBmZLmDwDfMNGAUcdRnjlC+few4MCcaIUODMCVwaHRxoJx/A==', N'+2349025055210', CAST(1 AS bit), NULL, NULL, N'81b94cda-96bb-43e0-ac86-6d4a3de474f9', NULL, CAST(0 AS bit), NULL, N'frontdesk@innercircle.com', NULL, NULL),
('ca5eb7a4-de1e-40a1-9c58-ac452112aa92', 0, CAST(1 AS bit), N'd6b62f0f-eec1-437a-9faf-0c93b254fd37', NULL, '2022-10-15T00:00:00.0000000', NULL, NULL, NULL, N'admin@innercircle.com', CAST(1 AS bit), N'', NULL, CAST(0 AS bit), NULL, '2022-10-15T00:00:00.0000000', N'Admin', CAST(0 AS bit), NULL, NULL, NULL, NULL, N'ADMIN@INNERCIRCLE.COM', N'ADMIN@INNERCIRCLE.COM', N'AQAAAAEAACcQAAAAEBGk1G9TSAb/LxllQr9/h2PrR8cHtHu+JKnN42LTbDFTe1ZtRiEq4+/mZjYfVaizww==', N'09025055210', CAST(1 AS bit), NULL, NULL, N'd2db0156-280e-4867-9795-8303362024dd', NULL, CAST(0 AS bit), NULL, N'admin@innercircle.com', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AccessFailedCount', N'Activated', N'ConcurrencyStamp', N'CreatedBy', N'CreatedOn', N'DeletedBy', N'DeletedOn', N'Department', N'Email', N'EmailConfirmed', N'FirstName', N'Gender', N'IsDeleted', N'IsPasswordDefault', N'LastLoginDate', N'LastName', N'LockoutEnabled', N'LockoutEnd', N'MiddleName', N'ModifiedBy', N'ModifiedOn', N'NormalizedEmail', N'NormalizedUserName', N'PasswordHash', N'PhoneNumber', N'PhoneNumberConfirmed', N'ProviderKey', N'RefreshToken', N'SecurityStamp', N'StaffNo', N'TwoFactorEnabled', N'Unit', N'UserName', N'UserType', N'UserTypeId') AND [object_id] = OBJECT_ID(N'[AppUsers]'))
    SET IDENTITY_INSERT [AppUsers] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'UserId') AND [object_id] = OBJECT_ID(N'[AppUserRoles]'))
    SET IDENTITY_INSERT [AppUserRoles] ON;
INSERT INTO [AppUserRoles] ([RoleId], [UserId])
VALUES ('ca7061a2-138c-45b7-870c-699caa9ca99b', '1743b5bd-1eb1-45b3-9630-99596b17cf53');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'UserId') AND [object_id] = OBJECT_ID(N'[AppUserRoles]'))
    SET IDENTITY_INSERT [AppUserRoles] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'UserId') AND [object_id] = OBJECT_ID(N'[AppUserRoles]'))
    SET IDENTITY_INSERT [AppUserRoles] ON;
INSERT INTO [AppUserRoles] ([RoleId], [UserId])
VALUES ('773a3af2-cd9f-4f65-869f-0cfdc1e1589e', '50b70c44-9eb7-4549-9a48-7d37809b7d8e');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'UserId') AND [object_id] = OBJECT_ID(N'[AppUserRoles]'))
    SET IDENTITY_INSERT [AppUserRoles] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'UserId') AND [object_id] = OBJECT_ID(N'[AppUserRoles]'))
    SET IDENTITY_INSERT [AppUserRoles] ON;
INSERT INTO [AppUserRoles] ([RoleId], [UserId])
VALUES ('cc785f2a-2c0a-4648-87b7-a500084a2c1a', 'ca5eb7a4-de1e-40a1-9c58-ac452112aa92');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'UserId') AND [object_id] = OBJECT_ID(N'[AppUserRoles]'))
    SET IDENTITY_INSERT [AppUserRoles] OFF;
GO

CREATE INDEX [IX_AppRoleClaims_RoleId] ON [AppRoleClaims] ([RoleId]);
GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [AppRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
GO

CREATE INDEX [IX_AppUserClaims_UserId] ON [AppUserClaims] ([UserId]);
GO

CREATE INDEX [IX_AppUserLogins_UserId] ON [AppUserLogins] ([UserId]);
GO

CREATE INDEX [IX_AppUserRoles_RoleId] ON [AppUserRoles] ([RoleId]);
GO

CREATE INDEX [EmailIndex] ON [AppUsers] ([NormalizedEmail]);
GO

CREATE UNIQUE INDEX [UserNameIndex] ON [AppUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
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

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230712234641_initial', N'6.0.12');
GO

COMMIT;
GO

