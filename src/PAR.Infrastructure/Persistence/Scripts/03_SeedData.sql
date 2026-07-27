-- ============================================================
-- PAR - Datos iniciales (roles, permisos y usuario admin)
-- ============================================================

USE PARDB;
GO

-- ─────────────────────────────────────────────────────────────
-- ROLES
-- ─────────────────────────────────────────────────────────────
SET IDENTITY_INSERT [dbo].[Roles] ON;

MERGE INTO [dbo].[Roles] AS target
USING (VALUES
    (1, 'Admin', 'Administrador del sistema', 1),
    (2, 'User',  'Usuario estándar',          1)
) AS source ([Id], [Name], [Description], [IsActive])
ON target.[Id] = source.[Id]
WHEN NOT MATCHED THEN
    INSERT ([Id], [Name], [Description], [IsActive])
    VALUES (source.[Id], source.[Name], source.[Description], source.[IsActive]);

SET IDENTITY_INSERT [dbo].[Roles] OFF;
PRINT 'Roles insertados.';
GO

-- ─────────────────────────────────────────────────────────────
-- PERMISSIONS
-- ─────────────────────────────────────────────────────────────
SET IDENTITY_INSERT [dbo].[Permissions] ON;

MERGE INTO [dbo].[Permissions] AS target
USING (VALUES
    (1, 'users.read',   'Users', 'Ver listado de usuarios'),
    (2, 'users.create', 'Users', 'Crear nuevos usuarios'),
    (3, 'users.update', 'Users', 'Editar usuarios existentes'),
    (4, 'users.delete', 'Users', 'Eliminar usuarios'),
    (5, 'users.lock',   'Users', 'Bloquear y desbloquear usuarios')
) AS source ([Id], [Name], [Module], [Description])
ON target.[Id] = source.[Id]
WHEN NOT MATCHED THEN
    INSERT ([Id], [Name], [Module], [Description])
    VALUES (source.[Id], source.[Name], source.[Module], source.[Description]);

SET IDENTITY_INSERT [dbo].[Permissions] OFF;
PRINT 'Permisos insertados.';
GO

-- ─────────────────────────────────────────────────────────────
-- ROLE_PERMISSIONS
--   Admin  → todos los permisos (1-5)
--   User   → solo lectura (1)
-- ─────────────────────────────────────────────────────────────
MERGE INTO [dbo].[RolePermissions] AS target
USING (VALUES
    (1, 1), (1, 2), (1, 3), (1, 4), (1, 5),  -- Admin
    (2, 1)                                     -- User
) AS source ([RoleId], [PermissionId])
ON target.[RoleId] = source.[RoleId]
AND target.[PermissionId] = source.[PermissionId]
WHEN NOT MATCHED THEN
    INSERT ([RoleId], [PermissionId])
    VALUES (source.[RoleId], source.[PermissionId]);

PRINT 'RolePermissions insertados.';
GO

-- ─────────────────────────────────────────────────────────────
-- USUARIO ADMIN POR DEFECTO
-- ⚠ Este script SQL es solo referencia. El usuario admin
--   se crea automáticamente al arrancar la API con la
--   contraseña correcta generada por BCrypt (DatabaseSeeder).
--   Credenciales: usuario=admin / contraseña=Admin@123
-- ─────────────────────────────────────────────────────────────
IF NOT EXISTS (SELECT 1 FROM [dbo].[Users] WHERE [Username] = 'admin')
BEGIN
    DECLARE @AdminId INT;

    INSERT INTO [dbo].[Users]
        ([Username], [Email], [PasswordHash], [FirstName], [LastName], [IsActive], [IsLocked], [CreatedAt])
    VALUES
        ('admin',
         'admin@par.com',
         '$2a$12$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', -- Admin@123
         'Administrador',
         'Sistema',
         1, 0,
         GETUTCDATE());

    SET @AdminId = SCOPE_IDENTITY();

    -- Asignar rol Admin
    INSERT INTO [dbo].[UserRoles] ([UserId], [RoleId])
    VALUES (@AdminId, 1);

    -- Registrar en historial de contraseñas
    INSERT INTO [dbo].[UserPasswordHistories] ([UserId], [PasswordHash], [CreatedAt])
    VALUES (@AdminId, '$2a$12$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', GETUTCDATE());

    PRINT 'Usuario admin creado (user: admin / pass: Admin@123).';
END
ELSE
    PRINT 'El usuario admin ya existe.';
GO

PRINT '✔ Seed data aplicado correctamente.';
GO
