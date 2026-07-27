namespace PAR.Application.Features.Users.DTOs;

public class UserPermissionSummaryDto
{
    public IEnumerable<int> RolePermissionIds   { get; set; } = [];
    public IEnumerable<int> DirectPermissionIds { get; set; } = [];
}
