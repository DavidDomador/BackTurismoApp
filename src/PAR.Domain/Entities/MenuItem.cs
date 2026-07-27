namespace PAR.Domain.Entities;

public class MenuItem
{
    public int Id { get; set; }
    public int MenuId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Route { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public string? RequiredPermission { get; set; }
    public int Order { get; set; }
    public bool IsActive { get; set; } = true;
    public Menu Menu { get; set; } = null!;
}
