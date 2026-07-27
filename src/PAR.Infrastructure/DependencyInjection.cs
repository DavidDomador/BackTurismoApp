using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PAR.Application.Ports;
using PAR.Domain.Ports;
using PAR.Infrastructure.Adapters.Persistence;
using PAR.Infrastructure.Adapters.Services;
using PAR.Infrastructure.Persistence;
using PAR.Shared.Excel;
using PAR.Shared.PDF;

namespace PAR.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

        // Adapters secundarios (driven): repositorios y servicios externos
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ILoginAttemptRepository, LoginAttemptRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IMenuRepository, MenuRepository>();
        services.AddScoped<IGuiaRepository, GuiaRepository>();
        services.AddScoped<ITransporteRepository, TransporteRepository>();
        services.AddScoped<IDefinicionRepository, DefinicionRepository>();
        services.AddScoped<IPaqueteRepository, PaqueteRepository>();
        services.AddScoped<IItinerarioRepository, ItinerarioRepository>();
        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<IReservaRepository, ReservaRepository>();
        services.AddScoped<IReservaDetalleRepository, ReservaDetalleRepository>();
        services.AddScoped<IEmpresaRepository, EmpresaRepository>();
        services.AddScoped<IVentaRepository, VentaRepository>();
        services.AddScoped<IVentaDetalleRepository, VentaDetalleRepository>();
        services.AddScoped<ISalidaRepository, SalidaRepository>();
        services.AddScoped<ISalidaExcelService, SalidaExcelService>();
        services.AddScoped<IPaquetePdfService, PaquetePdfService>();
        services.AddScoped<IPdfMergerService, PdfMergerService>();
        services.AddScoped<IReservaPdfService, ReservaPdfService>();
        services.AddScoped<IVentaPdfService, VentaPdfService>();
        services.AddScoped<IExcelExportService, ExcelExportService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<DatabaseSeeder>();

        return services;
    }
}
