using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PAR.Application.Ports;
using PAR.Domain.Entities;

namespace PAR.Infrastructure.Persistence;

public class DatabaseSeeder(
    AppDbContext context,
    IPasswordService passwordService,
    ILogger<DatabaseSeeder> logger)
{
    public async Task SeedAsync()
    {
        // Aplica migraciones pendientes automáticamente
        await context.Database.MigrateAsync();

        await SeedAdminUserAsync();
    }

    private async Task SeedAdminUserAsync()
    {
        if (await context.Users.AnyAsync())
            return;

        logger.LogInformation("No se encontraron usuarios. Creando usuario administrador inicial...");

        const string defaultPassword = "Admin@123";
        var hash = passwordService.HashPassword(defaultPassword);

        var admin = User.Create("admin", "admin@par.com", hash, "Administrador", "Sistema");

        context.Users.Add(admin);
        await context.SaveChangesAsync();

        // Asignar rol Admin (Id = 1, creado por EF seed)
        context.UserRoles.Add(new UserRole { UserId = admin.Id, RoleId = 1 });

        // Registrar en historial de contraseñas
        context.UserPasswordHistories.Add(new UserPasswordHistory
        {
            UserId       = admin.Id,
            PasswordHash = hash,
            CreatedAt    = DateTime.UtcNow
        });

        await context.SaveChangesAsync();

        logger.LogInformation("Usuario administrador creado. Usuario: admin | Contraseña: {Password}", defaultPassword);
    }
}
