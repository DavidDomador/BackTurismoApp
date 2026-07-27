-- ============================================================
-- PAR - Creación de tablas
-- ============================================================

USE PARDB;
GO

-- ─────────────────────────────────────────────────────────────
-- ROLES
-- ─────────────────────────────────────────────────────────────
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Roles')
BEGIN
    CREATE TABLE [dbo].[Roles] (
        [Id]          INT            NOT NULL IDENTITY(1,1),
        [Name]        NVARCHAR(100)  NOT NULL,
        [Description] NVARCHAR(300)  NULL,
        [IsActive]    BIT            NOT NULL CONSTRAINT DF_Roles_IsActive DEFAULT (1),

        CONSTRAINT PK_Roles PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT UQ_Roles_Name UNIQUE ([Name])
    );
    PRINT 'Tabla Roles creada.';
END
GO

-- ─────────────────────────────────────────────────────────────
-- PERMISSIONS
-- ─────────────────────────────────────────────────────────────
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Permissions')
BEGIN
    CREATE TABLE [dbo].[Permissions] (
        [Id]          INT            NOT NULL IDENTITY(1,1),
        [Name]        NVARCHAR(100)  NOT NULL,
        [Module]      NVARCHAR(100)  NOT NULL,
        [Description] NVARCHAR(300)  NULL,

        CONSTRAINT PK_Permissions PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT UQ_Permissions_Name UNIQUE ([Name])
    );
    PRINT 'Tabla Permissions creada.';
END
GO

-- ─────────────────────────────────────────────────────────────
-- USERS
-- ─────────────────────────────────────────────────────────────
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
BEGIN
    CREATE TABLE [dbo].[Users] (
        [Id]           INT            NOT NULL IDENTITY(1,1),
        [Username]     NVARCHAR(100)  NOT NULL,
        [Email]        NVARCHAR(200)  NOT NULL,
        [PasswordHash] NVARCHAR(500)  NOT NULL,
        [FirstName]    NVARCHAR(100)  NULL,
        [LastName]     NVARCHAR(100)  NULL,
        [IsActive]     BIT            NOT NULL CONSTRAINT DF_Users_IsActive  DEFAULT (1),
        [IsLocked]     BIT            NOT NULL CONSTRAINT DF_Users_IsLocked  DEFAULT (0),
        [CreatedAt]    DATETIME2(7)   NOT NULL CONSTRAINT DF_Users_CreatedAt DEFAULT (GETUTCDATE()),
        [UpdatedAt]    DATETIME2(7)   NULL,

        CONSTRAINT PK_Users          PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT UQ_Users_Username UNIQUE ([Username]),
        CONSTRAINT UQ_Users_Email    UNIQUE ([Email])
    );
    PRINT 'Tabla Users creada.';
END
GO

-- ─────────────────────────────────────────────────────────────
-- USER_PASSWORD_HISTORIES
-- Historial de las últimas 3 contraseñas por usuario
-- ─────────────────────────────────────────────────────────────
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UserPasswordHistories')
BEGIN
    CREATE TABLE [dbo].[UserPasswordHistories] (
        [Id]           INT           NOT NULL IDENTITY(1,1),
        [UserId]       INT           NOT NULL,
        [PasswordHash] NVARCHAR(500) NOT NULL,
        [CreatedAt]    DATETIME2(7)  NOT NULL CONSTRAINT DF_UPH_CreatedAt DEFAULT (GETUTCDATE()),

        CONSTRAINT PK_UserPasswordHistories  PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT FK_UPH_Users
            FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([Id])
            ON DELETE CASCADE
    );

    CREATE NONCLUSTERED INDEX IX_UserPasswordHistories_UserId_CreatedAt
        ON [dbo].[UserPasswordHistories] ([UserId] ASC, [CreatedAt] DESC);

    PRINT 'Tabla UserPasswordHistories creada.';
END
GO

-- ─────────────────────────────────────────────────────────────
-- LOGIN_ATTEMPTS
-- Registro de intentos de login para brute-force protection
-- ─────────────────────────────────────────────────────────────
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'LoginAttempts')
BEGIN
    CREATE TABLE [dbo].[LoginAttempts] (
        [Id]            INT            NOT NULL IDENTITY(1,1),
        [UserId]        INT            NULL,
        [Username]      NVARCHAR(100)  NOT NULL,
        [IpAddress]     NVARCHAR(50)   NOT NULL,
        [Success]       BIT            NOT NULL,
        [FailureReason] NVARCHAR(300)  NULL,
        [AttemptedAt]   DATETIME2(7)   NOT NULL CONSTRAINT DF_LA_AttemptedAt DEFAULT (GETUTCDATE()),

        CONSTRAINT PK_LoginAttempts PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT FK_LoginAttempts_Users
            FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([Id])
            ON DELETE SET NULL
    );

    -- Índices para las consultas de brute-force (por IP y por username)
    CREATE NONCLUSTERED INDEX IX_LoginAttempts_IpAddress_AttemptedAt
        ON [dbo].[LoginAttempts] ([IpAddress] ASC, [AttemptedAt] DESC)
        INCLUDE ([Success]);

    CREATE NONCLUSTERED INDEX IX_LoginAttempts_Username_AttemptedAt
        ON [dbo].[LoginAttempts] ([Username] ASC, [AttemptedAt] DESC)
        INCLUDE ([Success]);

    PRINT 'Tabla LoginAttempts creada.';
END
GO

-- ─────────────────────────────────────────────────────────────
-- USER_ROLES  (tabla pivote N:N)
-- ─────────────────────────────────────────────────────────────
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UserRoles')
BEGIN
    CREATE TABLE [dbo].[UserRoles] (
        [UserId] INT NOT NULL,
        [RoleId] INT NOT NULL,

        CONSTRAINT PK_UserRoles
            PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC),
        CONSTRAINT FK_UserRoles_Users
            FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([Id])
            ON DELETE CASCADE,
        CONSTRAINT FK_UserRoles_Roles
            FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles]([Id])
            ON DELETE CASCADE
    );
    PRINT 'Tabla UserRoles creada.';
END
GO

-- ─────────────────────────────────────────────────────────────
-- ROLE_PERMISSIONS  (tabla pivote N:N)
-- ─────────────────────────────────────────────────────────────
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'RolePermissions')
BEGIN
    CREATE TABLE [dbo].[RolePermissions] (
        [RoleId]       INT NOT NULL,
        [PermissionId] INT NOT NULL,

        CONSTRAINT PK_RolePermissions
            PRIMARY KEY CLUSTERED ([RoleId] ASC, [PermissionId] ASC),
        CONSTRAINT FK_RolePermissions_Roles
            FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles]([Id])
            ON DELETE CASCADE,
        CONSTRAINT FK_RolePermissions_Permissions
            FOREIGN KEY ([PermissionId]) REFERENCES [dbo].[Permissions]([Id])
            ON DELETE CASCADE
    );
    PRINT 'Tabla RolePermissions creada.';
END
GO

PRINT '✔ Todas las tablas fueron creadas correctamente.';
GO
