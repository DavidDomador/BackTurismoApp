namespace PAR.Domain.Entities;

/// <summary>
/// Aggregate Root del dominio de usuarios.
/// Toda la lógica de negocio que afecta al usuario se encapsula aquí.
/// </summary>
public class User
{
    public int Id { get; private set; }
    public string Username { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public bool IsActive { get; private set; } = true;
    public bool IsLocked { get; private set; } = false;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; private set; }
    public ICollection<UserPasswordHistory> PasswordHistories { get; private set; } = new List<UserPasswordHistory>();
    public ICollection<LoginAttempt> LoginAttempts { get; private set; } = new List<LoginAttempt>();
    public ICollection<UserRole> UserRoles { get; private set; } = new List<UserRole>();
    public ICollection<UserPermission> UserPermissions { get; private set; } = new List<UserPermission>();

    // Constructor sin parámetros requerido por EF Core
    private User() { }

    /// <summary>Factory method: crea un nuevo usuario con los datos validados.</summary>
    public static User Create(string username, string email, string passwordHash, string? firstName, string? lastName) =>
        new()
        {
            Username = username,
            Email = email,
            PasswordHash = passwordHash,
            FirstName = firstName,
            LastName = lastName,
            IsActive = true,
            IsLocked = false,
            CreatedAt = DateTime.UtcNow
        };

    /// <summary>Bloquea la cuenta del usuario.</summary>
    public void Lock()
    {
        IsLocked = true;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>Desbloquea la cuenta del usuario.</summary>
    public void Unlock()
    {
        IsLocked = false;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>Cambia el hash de contraseña del usuario.</summary>
    public void ChangePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>Actualiza los datos del perfil del usuario.</summary>
    public void UpdateDetails(string email, string? firstName, string? lastName, bool isActive)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        IsActive = isActive;
        UpdatedAt = DateTime.UtcNow;
    }
}
