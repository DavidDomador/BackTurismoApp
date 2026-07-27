CREATE OR ALTER PROCEDURE [dbo].[SP_GetLoginAttemptsSummary]
    @IpAddress NVARCHAR(50) = NULL,
    @Username NVARCHAR(100) = NULL,
    @WindowMinutes INT = 15
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Since DATETIME2 = DATEADD(MINUTE, -@WindowMinutes, GETUTCDATE());

    SELECT
        Username,
        IpAddress,
        COUNT(*) AS TotalAttempts,
        SUM(CASE WHEN Success = 0 THEN 1 ELSE 0 END) AS FailedAttempts,
        SUM(CASE WHEN Success = 1 THEN 1 ELSE 0 END) AS SuccessAttempts,
        MAX(AttemptedAt) AS LastAttempt
    FROM LoginAttempts
    WHERE AttemptedAt >= @Since
        AND (@IpAddress IS NULL OR IpAddress = @IpAddress)
        AND (@Username IS NULL OR Username = @Username)
    GROUP BY Username, IpAddress
    ORDER BY TotalAttempts DESC;
END
