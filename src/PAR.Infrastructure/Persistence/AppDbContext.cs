using Microsoft.EntityFrameworkCore;
using PAR.Domain.Entities;

namespace PAR.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<UserPasswordHistory> UserPasswordHistories => Set<UserPasswordHistory>();
    public DbSet<LoginAttempt> LoginAttempts => Set<LoginAttempt>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<Menu> Menus => Set<Menu>();
    public DbSet<MenuItem> MenuItems => Set<MenuItem>();
    public DbSet<UserPermission> UserPermissions => Set<UserPermission>();
    public DbSet<Guia> Guias => Set<Guia>();
    public DbSet<Chofer> Choferes => Set<Chofer>();
    public DbSet<Vehiculo> Vehiculos => Set<Vehiculo>();
    public DbSet<ChoferVehiculo> ChoferVehiculos => Set<ChoferVehiculo>();
    public DbSet<Definicion> Definiciones => Set<Definicion>();
    public DbSet<DefinicionDetalle> DefinicionDetalles => Set<DefinicionDetalle>();
    public DbSet<Paquete> Paquetes => Set<Paquete>();
    public DbSet<Itinerario> Itinerarios => Set<Itinerario>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Reserva> Reservas => Set<Reserva>();
    public DbSet<ReservaDetalle> ReservaDetalles => Set<ReservaDetalle>();
    public DbSet<Empresa> Empresas => Set<Empresa>();
    public DbSet<Venta> Ventas => Set<Venta>();
    public DbSet<VentaDetalle> VentaDetalles => Set<VentaDetalle>();
    public DbSet<Salida> Salidas => Set<Salida>();
    public DbSet<SalidaReserva> SalidaReservas => Set<SalidaReserva>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Email).HasMaxLength(200).IsRequired();
            entity.Property(e => e.PasswordHash).HasMaxLength(500).IsRequired();
            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
        });

        modelBuilder.Entity<UserPasswordHistory>(entity =>
        {
            entity.ToTable("UserPasswordHistories");
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.User)
                .WithMany(u => u.PasswordHistories)
                .HasForeignKey(e => e.UserId);
        });

        modelBuilder.Entity<LoginAttempt>(entity =>
        {
            entity.ToTable("LoginAttempts");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.IpAddress).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(100);
            entity.HasIndex(e => new { e.IpAddress, e.AttemptedAt });
            entity.HasIndex(e => new { e.Username, e.AttemptedAt });
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Roles");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.ToTable("Permissions");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Module).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.ToTable("UserRoles");
            entity.HasKey(e => new { e.UserId, e.RoleId });
            entity.HasOne(e => e.User).WithMany(u => u.UserRoles).HasForeignKey(e => e.UserId);
            entity.HasOne(e => e.Role).WithMany(r => r.UserRoles).HasForeignKey(e => e.RoleId);
        });

        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.ToTable("RolePermissions");
            entity.HasKey(e => new { e.RoleId, e.PermissionId });
            entity.HasOne(e => e.Role).WithMany(r => r.RolePermissions).HasForeignKey(e => e.RoleId);
            entity.HasOne(e => e.Permission).WithMany(p => p.RolePermissions).HasForeignKey(e => e.PermissionId);
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.ToTable("Menus");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Icon).HasMaxLength(100);
        });

        modelBuilder.Entity<MenuItem>(entity =>
        {
            entity.ToTable("MenuItems");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Route).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Icon).HasMaxLength(100);
            entity.Property(e => e.RequiredPermission).HasMaxLength(100);
            entity.HasOne(e => e.Menu)
                .WithMany(m => m.Items)
                .HasForeignKey(e => e.MenuId);
        });

        modelBuilder.Entity<UserPermission>(entity =>
        {
            entity.ToTable("UserPermissions");
            entity.HasKey(e => new { e.UserId, e.PermissionId });
            entity.HasOne(e => e.User).WithMany(u => u.UserPermissions).HasForeignKey(e => e.UserId);
            entity.HasOne(e => e.Permission).WithMany(p => p.UserPermissions).HasForeignKey(e => e.PermissionId);
        });

        modelBuilder.Entity<Guia>(entity =>
        {
            entity.ToTable("Guia");
            entity.HasKey(e => e.ICodigoGuia);
            entity.Property(e => e.ICodigoGuia).HasColumnName("iCodigoGuia").ValueGeneratedOnAdd();
            entity.Property(e => e.GNombre).HasColumnName("gNombre").HasMaxLength(100).IsRequired();
            entity.Property(e => e.GApellidos).HasColumnName("gApellidos").HasMaxLength(100).IsRequired();
            entity.Property(e => e.GDni).HasColumnName("gDni").HasMaxLength(20).IsRequired();
            entity.Property(e => e.GCorreo).HasColumnName("gCorreo").HasMaxLength(200).IsRequired();
        });

        modelBuilder.Entity<Chofer>(entity =>
        {
            entity.ToTable("Choferes");
            entity.HasKey(e => e.ICodigoChofer);
            entity.Property(e => e.ICodigoChofer).HasColumnName("iCodigoChofer").ValueGeneratedOnAdd();
            entity.Property(e => e.GNombre).HasColumnName("gNombre").HasMaxLength(100).IsRequired();
            entity.Property(e => e.GApellidos).HasColumnName("gApellidos").HasMaxLength(100).IsRequired();
            entity.Property(e => e.GDni).HasColumnName("gDni").HasMaxLength(20).IsRequired();
            entity.Property(e => e.GLicencia).HasColumnName("gLicencia").HasMaxLength(50).IsRequired();
            entity.Property(e => e.GTelefono).HasColumnName("gTelefono").HasMaxLength(20);
        });

        modelBuilder.Entity<Vehiculo>(entity =>
        {
            entity.ToTable("Vehiculos");
            entity.HasKey(e => e.ICodigoVehiculo);
            entity.Property(e => e.ICodigoVehiculo).HasColumnName("iCodigoVehiculo").ValueGeneratedOnAdd();
            entity.Property(e => e.GPlaca).HasColumnName("gPlaca").HasMaxLength(20).IsRequired();
            entity.Property(e => e.GMarca).HasColumnName("gMarca").HasMaxLength(100).IsRequired();
            entity.Property(e => e.GModelo).HasColumnName("gModelo").HasMaxLength(100).IsRequired();
            entity.Property(e => e.GAnio).HasColumnName("gAnio");
            entity.Property(e => e.GColor).HasColumnName("gColor").HasMaxLength(50);
            entity.Property(e => e.GCantidadAsientos).HasColumnName("gCantidadAsientos");
        });

        modelBuilder.Entity<ChoferVehiculo>(entity =>
        {
            entity.ToTable("ChoferVehiculo");
            entity.HasKey(e => e.ICodigoChoferVehiculo);
            entity.Property(e => e.ICodigoChoferVehiculo).HasColumnName("iCodigoChoferVehiculo").ValueGeneratedOnAdd();
            entity.Property(e => e.ChoferID).HasColumnName("ChoferID");
            entity.Property(e => e.VehiculoID).HasColumnName("VehiculoID");
            entity.HasOne(e => e.Chofer).WithMany(c => c.ChoferVehiculos).HasForeignKey(e => e.ChoferID);
            entity.HasOne(e => e.Vehiculo).WithMany(v => v.ChoferVehiculos).HasForeignKey(e => e.VehiculoID);
        });

        // Seed data
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Admin", Description = "Administrator", IsActive = true },
            new Role { Id = 2, Name = "User", Description = "Standard user", IsActive = true }
        );

        modelBuilder.Entity<Permission>().HasData(
            new Permission { Id = 1,  Name = "users.read",        Module = "Users",      Description = "View users" },
            new Permission { Id = 2,  Name = "users.create",      Module = "Users",      Description = "Create users" },
            new Permission { Id = 3,  Name = "users.update",      Module = "Users",      Description = "Update users" },
            new Permission { Id = 4,  Name = "users.delete",      Module = "Users",      Description = "Delete users" },
            new Permission { Id = 5,  Name = "users.lock",        Module = "Users",      Description = "Lock/unlock users" },
            new Permission { Id = 6,  Name = "users.permissions", Module = "Config",     Description = "Manage user roles and permissions" },
            new Permission { Id = 7,  Name = "guia.read",         Module = "Guia",       Description = "View guias" },
            new Permission { Id = 8,  Name = "guia.create",       Module = "Guia",       Description = "Create guias" },
            new Permission { Id = 9,  Name = "guia.update",       Module = "Guia",       Description = "Update guias" },
            new Permission { Id = 10, Name = "guia.delete",       Module = "Guia",       Description = "Delete guias" },
            new Permission { Id = 11, Name = "transporte.read",   Module = "Transporte",  Description = "View transporte" },
            new Permission { Id = 12, Name = "transporte.create", Module = "Transporte",  Description = "Create transporte" },
            new Permission { Id = 13, Name = "transporte.update", Module = "Transporte",  Description = "Update transporte" },
            new Permission { Id = 14, Name = "transporte.delete", Module = "Transporte",  Description = "Delete transporte" },
            new Permission { Id = 15, Name = "definicion.read",   Module = "Definicion",  Description = "View definiciones" },
            new Permission { Id = 16, Name = "definicion.create", Module = "Definicion",  Description = "Create definiciones" },
            new Permission { Id = 17, Name = "definicion.update", Module = "Definicion",  Description = "Update definiciones" },
            new Permission { Id = 18, Name = "definicion.delete", Module = "Definicion",  Description = "Delete definiciones" },
            new Permission { Id = 19, Name = "paquete.read",     Module = "Paquete",     Description = "View paquetes" },
            new Permission { Id = 20, Name = "paquete.create",   Module = "Paquete",     Description = "Create paquetes" },
            new Permission { Id = 21, Name = "paquete.update",   Module = "Paquete",     Description = "Update paquetes" },
            new Permission { Id = 22, Name = "paquete.delete",   Module = "Paquete",     Description = "Delete paquetes" },
            new Permission { Id = 23, Name = "cliente.read",     Module = "Cliente",     Description = "View clientes" },
            new Permission { Id = 24, Name = "cliente.create",   Module = "Cliente",     Description = "Create clientes" },
            new Permission { Id = 25, Name = "cliente.update",   Module = "Cliente",     Description = "Update clientes" },
            new Permission { Id = 26, Name = "cliente.delete",   Module = "Cliente",     Description = "Delete clientes" },
            new Permission { Id = 27, Name = "reserva.read",     Module = "Reserva",     Description = "View reservas" },
            new Permission { Id = 28, Name = "reserva.create",   Module = "Reserva",     Description = "Create reservas" },
            new Permission { Id = 29, Name = "reserva.update",   Module = "Reserva",     Description = "Update reservas" },
            new Permission { Id = 30, Name = "reserva.delete",   Module = "Reserva",     Description = "Delete reservas" }
        );

        modelBuilder.Entity<RolePermission>().HasData(
            new RolePermission { RoleId = 1, PermissionId = 1  },
            new RolePermission { RoleId = 1, PermissionId = 2  },
            new RolePermission { RoleId = 1, PermissionId = 3  },
            new RolePermission { RoleId = 1, PermissionId = 4  },
            new RolePermission { RoleId = 1, PermissionId = 5  },
            new RolePermission { RoleId = 1, PermissionId = 6  },
            new RolePermission { RoleId = 1, PermissionId = 7  },
            new RolePermission { RoleId = 1, PermissionId = 8  },
            new RolePermission { RoleId = 1, PermissionId = 9  },
            new RolePermission { RoleId = 1, PermissionId = 10 },
            new RolePermission { RoleId = 1, PermissionId = 11 },
            new RolePermission { RoleId = 1, PermissionId = 12 },
            new RolePermission { RoleId = 1, PermissionId = 13 },
            new RolePermission { RoleId = 1, PermissionId = 14 },
            new RolePermission { RoleId = 1, PermissionId = 15 },
            new RolePermission { RoleId = 1, PermissionId = 16 },
            new RolePermission { RoleId = 1, PermissionId = 17 },
            new RolePermission { RoleId = 1, PermissionId = 18 },
            new RolePermission { RoleId = 1, PermissionId = 19 },
            new RolePermission { RoleId = 1, PermissionId = 20 },
            new RolePermission { RoleId = 1, PermissionId = 21 },
            new RolePermission { RoleId = 1, PermissionId = 22 },
            new RolePermission { RoleId = 1, PermissionId = 23 },
            new RolePermission { RoleId = 1, PermissionId = 24 },
            new RolePermission { RoleId = 1, PermissionId = 25 },
            new RolePermission { RoleId = 1, PermissionId = 26 },
            new RolePermission { RoleId = 1, PermissionId = 27 },
            new RolePermission { RoleId = 1, PermissionId = 28 },
            new RolePermission { RoleId = 1, PermissionId = 29 },
            new RolePermission { RoleId = 1, PermissionId = 30 },
            new RolePermission { RoleId = 2, PermissionId = 1  }
        );

        modelBuilder.Entity<Menu>().HasData(
            new Menu { Id = 1, Name = "Administración", Icon = "admin",       Order = 1, IsActive = true },
            new Menu { Id = 2, Name = "Configuración",  Icon = "config",      Order = 2, IsActive = true },
            new Menu { Id = 3, Name = "Paquete",        Icon = "package",     Order = 3, IsActive = true },
            new Menu { Id = 4, Name = "Reserva",        Icon = "reservation", Order = 4, IsActive = true }
        );

        modelBuilder.Entity<MenuItem>().HasData(
            new MenuItem { Id = 1, MenuId = 1, Name = "Usuarios",           Route = "/admin/users",             Icon = "people", RequiredPermission = "users.read",           Order = 1, IsActive = true },
            new MenuItem { Id = 2, MenuId = 2, Name = "Usuario y Permisos", Route = "/config/user-permissions", Icon = "key",    RequiredPermission = "users.permissions",    Order = 1, IsActive = true },
            new MenuItem { Id = 3, MenuId = 1, Name = "Guia",               Route = "/admin/guia",              Icon = "book",   RequiredPermission = "guia.read",            Order = 2, IsActive = true },
            new MenuItem { Id = 4, MenuId = 1, Name = "Transporte",         Route = "/admin/transporte",        Icon = "truck",  RequiredPermission = "transporte.read",      Order = 3, IsActive = true },
            new MenuItem { Id = 5, MenuId = 2, Name = "Definiciones",       Route = "/config/definicion",       Icon = "list",   RequiredPermission = "definicion.read",      Order = 2, IsActive = true },
            new MenuItem { Id = 6, MenuId = 3, Name = "Paquetes",  Route = "/paquete/paquetes",   Icon = "box",      RequiredPermission = "paquete.read",   Order = 1, IsActive = true },
            new MenuItem { Id = 7, MenuId = 1, Name = "Clientes",  Route = "/admin/clientes",     Icon = "users2",   RequiredPermission = "cliente.read",   Order = 4, IsActive = true },
            new MenuItem { Id = 8, MenuId = 4, Name = "Reservas",  Route = "/reserva/reservas",   Icon = "calendar", RequiredPermission = "reserva.read",   Order = 1, IsActive = true }
        );

        modelBuilder.Entity<Definicion>(entity =>
        {
            entity.ToTable("Definicion");
            entity.HasKey(e => e.IdDef);
            entity.Property(e => e.IdDef).HasColumnName("idDef").ValueGeneratedOnAdd();
            entity.Property(e => e.DNombre).HasColumnName("dNombre").HasMaxLength(100).IsRequired();
            entity.Property(e => e.DDescripcion).HasColumnName("dDescripcion").HasMaxLength(500);
            entity.Property(e => e.Estado).HasColumnName("estado");
        });

        modelBuilder.Entity<DefinicionDetalle>(entity =>
        {
            entity.ToTable("DefinicionDetalle");
            entity.HasKey(e => e.IdDefD);
            entity.Property(e => e.IdDefD).HasColumnName("idDefD").ValueGeneratedOnAdd();
            entity.Property(e => e.DdValue).HasColumnName("ddValue").HasMaxLength(200).IsRequired();
            entity.Property(e => e.IdDef).HasColumnName("idDef");
            entity.Property(e => e.DdAbreviacion).HasColumnName("ddAbreviacion").HasMaxLength(50);
            entity.Property(e => e.DdDescripcion).HasColumnName("ddDescripcion").HasMaxLength(500);
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.HasOne(e => e.Definicion)
                .WithMany(d => d.Detalles)
                .HasForeignKey(e => e.IdDef);
        });

        modelBuilder.Entity<Paquete>(entity =>
        {
            entity.ToTable("Paquetes");
            entity.HasKey(e => e.ICodPaquete);
            entity.Property(e => e.ICodPaquete).HasColumnName("iCodPaquete").ValueGeneratedOnAdd();
            entity.Property(e => e.PNombre).HasColumnName("pNombre").HasMaxLength(200).IsRequired();
            entity.Property(e => e.PDescripcion).HasColumnName("pDescripcion").HasMaxLength(1000);
            entity.Property(e => e.PPrecioBase).HasColumnName("pPrecioBase").HasColumnType("decimal(18,2)");
            entity.Property(e => e.PPrecioDescuento).HasColumnName("pPrecioDescuento").HasColumnType("decimal(18,2)");
            entity.Property(e => e.PPrecioReserva).HasColumnName("pPrecioReserva").HasColumnType("decimal(18,2)");
            entity.Property(e => e.PdfAdjunto).HasColumnName("pdfAdjunto").HasMaxLength(260);
        });

        modelBuilder.Entity<Itinerario>(entity =>
        {
            entity.ToTable("Itinerario");
            entity.HasKey(e => e.ICodItinerario);
            entity.Property(e => e.ICodItinerario).HasColumnName("iCodItinerario").ValueGeneratedOnAdd();
            entity.Property(e => e.ICodPaquete).HasColumnName("iCodPaquete");
            entity.Property(e => e.INombre).HasColumnName("iNombre").HasMaxLength(200).IsRequired();
            entity.Property(e => e.IDescripcion).HasColumnName("iDescripcion").HasMaxLength(1000);
            entity.Property(e => e.Icon).HasColumnName("icon").HasMaxLength(100);
            entity.Property(e => e.Imagen).HasColumnName("imagen").HasMaxLength(260);
            entity.HasOne(e => e.Paquete)
                .WithMany(p => p.Itinerarios)
                .HasForeignKey(e => e.ICodPaquete);
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.ToTable("Clientes");
            entity.HasKey(e => e.ICodCliente);
            entity.Property(e => e.ICodCliente).HasColumnName("iCodCliente").ValueGeneratedOnAdd();
            entity.Property(e => e.CNombres).HasColumnName("cNombres").HasMaxLength(150).IsRequired();
            entity.Property(e => e.CApellidos).HasColumnName("cApellidos").HasMaxLength(150).IsRequired();
            entity.Property(e => e.CDni).HasColumnName("cDni").HasMaxLength(20).IsRequired();
            entity.Property(e => e.CCorreo).HasColumnName("cCorreo").HasMaxLength(200);
            entity.Property(e => e.CEdad).HasColumnName("cEdad");
            entity.Property(e => e.CDireccion).HasColumnName("cDireccion").HasMaxLength(500);
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.ToTable("Reservas");
            entity.HasKey(e => e.ICodReserva);
            entity.Property(e => e.ICodReserva).HasColumnName("iCodReserva").ValueGeneratedOnAdd();
            entity.Property(e => e.RNumeroReserva).HasColumnName("rNumeroReserva");
            entity.Property(e => e.ICodPaquete).HasColumnName("iCodPaquete");
            entity.Property(e => e.FechaTour).HasColumnName("fechaTour");
            entity.Property(e => e.RTotal).HasColumnName("rTotal").HasColumnType("decimal(18,2)");
            entity.Property(e => e.RAbono).HasColumnName("rAbono").HasColumnType("decimal(18,2)");
            entity.Property(e => e.RSaldo).HasColumnName("rSaldo").HasColumnType("decimal(18,2)");
            entity.Property(e => e.RIncluye).HasColumnName("rIncluye").HasMaxLength(2000);
            entity.Property(e => e.RNoIncluye).HasColumnName("rNoIncluye").HasMaxLength(2000);
            entity.Property(e => e.RTarifa).HasColumnName("rTarifa").HasColumnType("decimal(18,2)");
            entity.Property(e => e.RObservacion).HasColumnName("rObservacion").HasMaxLength(2000);
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.HasOne(e => e.Paquete)
                .WithMany()
                .HasForeignKey(e => e.ICodPaquete);
            entity.HasIndex(e => e.RNumeroReserva).IsUnique();
        });

        modelBuilder.Entity<ReservaDetalle>(entity =>
        {
            entity.ToTable("ReservaDetalle");
            entity.HasKey(e => e.ICodReservaDetalle);
            entity.Property(e => e.ICodReservaDetalle).HasColumnName("iCodReservaDetalle").ValueGeneratedOnAdd();
            entity.Property(e => e.ICodReserva).HasColumnName("iCodReserva");
            entity.Property(e => e.ICodCliente).HasColumnName("iCodCliente");
            entity.Property(e => e.EstadoCliente).HasColumnName("estadoCliente");
            entity.HasOne(e => e.Reserva)
                .WithMany(r => r.ReservaDetalles)
                .HasForeignKey(e => e.ICodReserva);
            entity.HasOne(e => e.Cliente)
                .WithMany()
                .HasForeignKey(e => e.ICodCliente);
            entity.HasOne(e => e.EstadoClienteDetalle)
                .WithMany()
                .HasForeignKey(e => e.EstadoCliente)
                .IsRequired(false);
        });

        modelBuilder.Entity<Empresa>(entity =>
        {
            entity.ToTable("Empresas");
            entity.HasKey(e => e.ICodEmpresa);
            entity.Property(e => e.ICodEmpresa).HasColumnName("iCodEmpresa").ValueGeneratedOnAdd();
            entity.Property(e => e.ERazonSocial).HasColumnName("eRazonSocial").HasMaxLength(300).IsRequired();
            entity.Property(e => e.EDireccion).HasColumnName("eDireccion").HasMaxLength(500);
            entity.Property(e => e.ETelefono).HasColumnName("eTelefono").HasMaxLength(30);
            entity.Property(e => e.ECorreo).HasColumnName("eCorreo").HasMaxLength(200);
            entity.Property(e => e.ERUC).HasColumnName("eRUC").HasMaxLength(20);
        });

        modelBuilder.Entity<Venta>(entity =>
        {
            entity.ToTable("Ventas");
            entity.HasKey(e => e.ICodVenta);
            entity.Property(e => e.ICodVenta).HasColumnName("iCodVenta").ValueGeneratedOnAdd();
            entity.Property(e => e.VNumeroVenta).HasColumnName("vNumeroVenta").HasMaxLength(20);
            entity.Property(e => e.ICodReserva).HasColumnName("iCodReserva");
            entity.Property(e => e.VFechaVenta).HasColumnName("vFechaVenta");
            entity.Property(e => e.ICodUsuario).HasColumnName("iCodUsuario");
            entity.Property(e => e.VUsuarioNombre).HasColumnName("vUsuarioNombre").HasMaxLength(200);
            entity.Property(e => e.ICodCliente).HasColumnName("iCodCliente");
            entity.Property(e => e.VTotal).HasColumnName("vTotal").HasColumnType("decimal(18,2)");
            entity.Property(e => e.VSaldo).HasColumnName("vSaldo").HasColumnType("decimal(18,2)").IsRequired(false);
            entity.HasOne(e => e.Reserva)
                .WithMany()
                .HasForeignKey(e => e.ICodReserva);
            entity.HasOne(e => e.Cliente)
                .WithMany()
                .HasForeignKey(e => e.ICodCliente)
                .IsRequired(false);
            entity.HasMany(e => e.Detalles)
                .WithOne(d => d.Venta)
                .HasForeignKey(d => d.ICodVenta);
        });

        modelBuilder.Entity<VentaDetalle>(entity =>
        {
            entity.ToTable("VentaDetalles");
            entity.HasKey(e => e.ICodVentaDetalle);
            entity.Property(e => e.ICodVentaDetalle).HasColumnName("iCodVentaDetalle").ValueGeneratedOnAdd();
            entity.Property(e => e.ICodVenta).HasColumnName("iCodVenta");
            entity.Property(e => e.ICodReservaDetalle).HasColumnName("iCodReservaDetalle");
            entity.Property(e => e.ICodCliente).HasColumnName("iCodCliente").IsRequired(false);
            entity.Property(e => e.VMonto).HasColumnName("vMonto").HasColumnType("decimal(18,2)");
            entity.Property(e => e.IsActive).HasColumnName("isActive").HasDefaultValue(true);
            entity.HasOne(e => e.Cliente)
                .WithMany()
                .HasForeignKey(e => e.ICodCliente)
                .IsRequired(false);
            entity.HasOne(e => e.ReservaDetalle)
                .WithMany()
                .HasForeignKey(e => e.ICodReservaDetalle);
        });

        // ── Seed: Permissions 31-38 ───────────────────────────────
        modelBuilder.Entity<Permission>().HasData(
            new Permission { Id = 31, Name = "empresa.read",   Module = "Empresa", Description = "View empresas" },
            new Permission { Id = 32, Name = "empresa.create", Module = "Empresa", Description = "Create empresas" },
            new Permission { Id = 33, Name = "empresa.update", Module = "Empresa", Description = "Update empresas" },
            new Permission { Id = 34, Name = "empresa.delete", Module = "Empresa", Description = "Delete empresas" },
            new Permission { Id = 35, Name = "venta.read",     Module = "Venta",   Description = "View ventas" },
            new Permission { Id = 36, Name = "venta.create",   Module = "Venta",   Description = "Create ventas" },
            new Permission { Id = 37, Name = "venta.update",   Module = "Venta",   Description = "Update ventas" },
            new Permission { Id = 38, Name = "venta.delete",   Module = "Venta",   Description = "Delete ventas" }
        );

        modelBuilder.Entity<RolePermission>().HasData(
            new RolePermission { RoleId = 1, PermissionId = 31 },
            new RolePermission { RoleId = 1, PermissionId = 32 },
            new RolePermission { RoleId = 1, PermissionId = 33 },
            new RolePermission { RoleId = 1, PermissionId = 34 },
            new RolePermission { RoleId = 1, PermissionId = 35 },
            new RolePermission { RoleId = 1, PermissionId = 36 },
            new RolePermission { RoleId = 1, PermissionId = 37 },
            new RolePermission { RoleId = 1, PermissionId = 38 }
        );

        // ── Seed: Menu Venta + MenuItems ──────────────────────────
        modelBuilder.Entity<Menu>().HasData(
            new Menu { Id = 5, Name = "Venta",     Icon = "venta",     Order = 5, IsActive = true },
            new Menu { Id = 6, Name = "Reportes",  Icon = "reportes",  Order = 6, IsActive = true }
        );

        modelBuilder.Entity<MenuItem>().HasData(
            new MenuItem { Id = 9,  MenuId = 1, Name = "Empresa",            Route = "/admin/empresa",           Icon = "building",  RequiredPermission = "empresa.read",          Order = 5, IsActive = true },
            new MenuItem { Id = 10, MenuId = 5, Name = "Ventas",             Route = "/venta/ventas",            Icon = "receipt",   RequiredPermission = "venta.read",            Order = 1, IsActive = true },
            new MenuItem { Id = 12, MenuId = 6, Name = "Reporte Reservas",   Route = "/reportes/reservas",       Icon = "chart-bar", RequiredPermission = "reporte.reservas",      Order = 1, IsActive = true },
            new MenuItem { Id = 13, MenuId = 6, Name = "Reporte Ventas",     Route = "/reportes/ventas",         Icon = "chart-bar", RequiredPermission = "reporte.ventas",        Order = 2, IsActive = true },
            new MenuItem { Id = 14, MenuId = 6, Name = "Reporte Salidas",    Route = "/reportes/salidas",        Icon = "chart-bar", RequiredPermission = "reporte.salidas",       Order = 3, IsActive = true }
        );

        // ── Seed: Permissions 39-41 (Reportes) ───────────────────
        modelBuilder.Entity<Permission>().HasData(
            new Permission { Id = 39, Name = "reporte.reservas", Module = "Reportes", Description = "View reporte reservas" },
            new Permission { Id = 40, Name = "reporte.ventas",   Module = "Reportes", Description = "View reporte ventas" },
            new Permission { Id = 41, Name = "reporte.salidas",  Module = "Reportes", Description = "View reporte salidas" }
        );

        modelBuilder.Entity<RolePermission>().HasData(
            new RolePermission { RoleId = 1, PermissionId = 39 },
            new RolePermission { RoleId = 1, PermissionId = 40 },
            new RolePermission { RoleId = 1, PermissionId = 41 }
        );

        // ── Seed: Permissions 42-45 (Salida CRUD) ────────────────
        modelBuilder.Entity<Permission>().HasData(
            new Permission { Id = 42, Name = "salida.read",   Module = "Operaciones", Description = "Ver salidas" },
            new Permission { Id = 43, Name = "salida.create", Module = "Operaciones", Description = "Crear salidas" },
            new Permission { Id = 44, Name = "salida.update", Module = "Operaciones", Description = "Editar salidas" },
            new Permission { Id = 45, Name = "salida.delete", Module = "Operaciones", Description = "Eliminar salidas" }
        );

        modelBuilder.Entity<RolePermission>().HasData(
            new RolePermission { RoleId = 1, PermissionId = 42 },
            new RolePermission { RoleId = 1, PermissionId = 43 },
            new RolePermission { RoleId = 1, PermissionId = 44 },
            new RolePermission { RoleId = 1, PermissionId = 45 }
        );

        // ── Salida entity ─────────────────────────────────────────
        modelBuilder.Entity<Salida>(entity =>
        {
            entity.ToTable("Salidas");
            entity.HasKey(e => e.ICodSalida);
            entity.Property(e => e.ICodSalida).HasColumnName("iCodSalida").ValueGeneratedOnAdd();
            entity.Property(e => e.ICodigoGuia).HasColumnName("iCodigoGuia");
            entity.Property(e => e.ICodChoferVehiculo).HasColumnName("iCodChoferVehiculo");
            entity.Property(e => e.FechaSalida).HasColumnName("fechaSalida");
            entity.Property(e => e.HoraSalida).HasColumnName("horaSalida").HasColumnType("time");
            entity.HasOne(e => e.Guia).WithMany().HasForeignKey(e => e.ICodigoGuia);
            entity.HasOne(e => e.ChoferVehiculo).WithMany().HasForeignKey(e => e.ICodChoferVehiculo);
            entity.HasMany(e => e.SalidaReservas).WithOne(sr => sr.Salida).HasForeignKey(sr => sr.ICodSalida);
        });

        // ── SalidaReserva junction ────────────────────────────────
        modelBuilder.Entity<SalidaReserva>(entity =>
        {
            entity.ToTable("SalidaReservas");
            entity.HasKey(e => e.ICodSalidaReserva);
            entity.Property(e => e.ICodSalidaReserva).HasColumnName("iCodSalidaReserva").ValueGeneratedOnAdd();
            entity.Property(e => e.ICodSalida).HasColumnName("iCodSalida");
            entity.Property(e => e.ICodReserva).HasColumnName("iCodReserva");
            entity.HasOne(e => e.Reserva).WithMany().HasForeignKey(e => e.ICodReserva);
        });
    }
}
