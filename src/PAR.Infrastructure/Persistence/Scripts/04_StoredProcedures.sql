-- ============================================================
-- PAR - Stored Procedures
-- ============================================================

USE PARDB;
GO

-- ─────────────────────────────────────────────────────────────
-- SP_GetUserWithRoles
-- Retorna un usuario con todos sus roles y permisos asociados
-- ─────────────────────────────────────────────────────────────
CREATE OR ALTER PROCEDURE [dbo].[SP_GetUserWithRoles]
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        u.Id,
        u.Username,
        u.Email,
        u.FirstName,
        u.LastName,
        u.IsActive,
        u.IsLocked,
        u.CreatedAt,
        u.UpdatedAt,
        r.Id           AS RoleId,
        r.Name         AS RoleName,
        p.Id           AS PermissionId,
        p.Name         AS PermissionName,
        p.Module       AS PermissionModule
    FROM [dbo].[Users] u
    LEFT JOIN [dbo].[UserRoles]       ur ON u.Id      = ur.UserId
    LEFT JOIN [dbo].[Roles]           r  ON ur.RoleId = r.Id
    LEFT JOIN [dbo].[RolePermissions] rp ON r.Id      = rp.RoleId
    LEFT JOIN [dbo].[Permissions]     p  ON rp.PermissionId = p.Id
    WHERE u.Id = @UserId;
END
GO

-- ─────────────────────────────────────────────────────────────
-- SP_GetLoginAttemptsSummary
-- Resumen de intentos de login en una ventana de tiempo
-- Parámetros opcionales: filtrar por IP y/o por Username
-- ─────────────────────────────────────────────────────────────
CREATE OR ALTER PROCEDURE [dbo].[SP_GetLoginAttemptsSummary]
    @IpAddress    NVARCHAR(50)  = NULL,
    @Username     NVARCHAR(100) = NULL,
    @WindowMinutes INT          = 15
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Since DATETIME2 = DATEADD(MINUTE, -@WindowMinutes, GETUTCDATE());

    SELECT
        la.Username,
        la.IpAddress,
        COUNT(*)                                         AS TotalAttempts,
        SUM(CASE WHEN la.Success = 0 THEN 1 ELSE 0 END) AS FailedAttempts,
        SUM(CASE WHEN la.Success = 1 THEN 1 ELSE 0 END) AS SuccessAttempts,
        MIN(la.AttemptedAt)                              AS FirstAttempt,
        MAX(la.AttemptedAt)                              AS LastAttempt
    FROM [dbo].[LoginAttempts] la
    WHERE la.AttemptedAt >= @Since
      AND (@IpAddress IS NULL OR la.IpAddress = @IpAddress)
      AND (@Username  IS NULL OR la.Username  = @Username)
    GROUP BY la.Username, la.IpAddress
    ORDER BY FailedAttempts DESC, LastAttempt DESC;
END
GO

PRINT '✔ Stored Procedures creados correctamente.';
GO
