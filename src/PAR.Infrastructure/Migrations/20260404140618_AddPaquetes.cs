using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PAR.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPaquetes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Paquetes",
                columns: table => new
                {
                    iCodPaquete = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    pNombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    pDescripcion = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    pPrecioBase = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    pPrecioDescuento = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    pPrecioReserva = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paquetes", x => x.iCodPaquete);
                });

            migrationBuilder.Sql(@"
                SET IDENTITY_INSERT [Menus] ON;
                IF NOT EXISTS (SELECT 1 FROM [Menus] WHERE [Id] = 3)
                    INSERT INTO [Menus] ([Id],[Icon],[IsActive],[Name],[Order]) VALUES (3,'package',1,'Paquete',3);
                SET IDENTITY_INSERT [Menus] OFF;

                SET IDENTITY_INSERT [Permissions] ON;
                IF NOT EXISTS (SELECT 1 FROM [Permissions] WHERE [Id] = 19)
                    INSERT INTO [Permissions] ([Id],[Description],[Module],[Name]) VALUES (19,'View paquetes','Paquete','paquete.read');
                IF NOT EXISTS (SELECT 1 FROM [Permissions] WHERE [Id] = 20)
                    INSERT INTO [Permissions] ([Id],[Description],[Module],[Name]) VALUES (20,'Create paquetes','Paquete','paquete.create');
                IF NOT EXISTS (SELECT 1 FROM [Permissions] WHERE [Id] = 21)
                    INSERT INTO [Permissions] ([Id],[Description],[Module],[Name]) VALUES (21,'Update paquetes','Paquete','paquete.update');
                IF NOT EXISTS (SELECT 1 FROM [Permissions] WHERE [Id] = 22)
                    INSERT INTO [Permissions] ([Id],[Description],[Module],[Name]) VALUES (22,'Delete paquetes','Paquete','paquete.delete');
                SET IDENTITY_INSERT [Permissions] OFF;

                SET IDENTITY_INSERT [MenuItems] ON;
                IF NOT EXISTS (SELECT 1 FROM [MenuItems] WHERE [Id] = 6)
                    INSERT INTO [MenuItems] ([Id],[Icon],[IsActive],[MenuId],[Name],[Order],[RequiredPermission],[Route])
                    VALUES (6,'box',1,3,'Paquetes',1,'paquete.read','/paquete/paquetes');
                SET IDENTITY_INSERT [MenuItems] OFF;

                IF NOT EXISTS (SELECT 1 FROM [RolePermissions] WHERE [RoleId]=1 AND [PermissionId]=19)
                    INSERT INTO [RolePermissions] ([RoleId],[PermissionId]) VALUES (1,19);
                IF NOT EXISTS (SELECT 1 FROM [RolePermissions] WHERE [RoleId]=1 AND [PermissionId]=20)
                    INSERT INTO [RolePermissions] ([RoleId],[PermissionId]) VALUES (1,20);
                IF NOT EXISTS (SELECT 1 FROM [RolePermissions] WHERE [RoleId]=1 AND [PermissionId]=21)
                    INSERT INTO [RolePermissions] ([RoleId],[PermissionId]) VALUES (1,21);
                IF NOT EXISTS (SELECT 1 FROM [RolePermissions] WHERE [RoleId]=1 AND [PermissionId]=22)
                    INSERT INTO [RolePermissions] ([RoleId],[PermissionId]) VALUES (1,22);
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Paquetes");

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 19, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 20, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 21, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 22, 1 });

            migrationBuilder.DeleteData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 22);
        }
    }
}
