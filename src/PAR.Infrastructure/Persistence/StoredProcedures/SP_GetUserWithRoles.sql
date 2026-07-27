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
        r.Name AS RoleName,
        p.Name AS PermissionName,
        p.Module AS PermissionModule
    FROM Users u
    LEFT JOIN UserRoles ur ON u.Id = ur.UserId
    LEFT JOIN Roles r ON ur.RoleId = r.Id
    LEFT JOIN RolePermissions rp ON r.Id = rp.RoleId
    LEFT JOIN Permissions p ON rp.PermissionId = p.Id
    WHERE u.Id = @UserId;
END
