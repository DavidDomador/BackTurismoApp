using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAR.Infrastructure.Migrations
{
    public partial class RestructurarMenuOperaciones : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                -- 1. Renombrar Menu 3 de 'Paquete' a 'Operaciones'
                UPDATE [Menus]
                SET [Name] = 'Operaciones', [Icon] = 'operaciones'
                WHERE [Id] = 3;

                -- 2. Mover 'Reservas' (MenuItem 9) de Menu 4 a Menu 3, orden 2
                UPDATE [MenuItems]
                SET [MenuId] = 3, [Order] = 2
                WHERE [Id] = 9;

                -- 3. Mover 'Ventas' (MenuItem 10) de Menu 5 a Menu 3, orden 3
                UPDATE [MenuItems]
                SET [MenuId] = 3, [Order] = 3
                WHERE [Id] = 11;

                -- 4. Eliminar los menús huérfanos (ya sin ítems)
                DELETE FROM [Menus] WHERE [Id] IN (4, 5);

                -- 5. Insertar sub-opción 'Salidas' bajo Operaciones (Menu 3)
                SET IDENTITY_INSERT [MenuItems] ON;
                IF NOT EXISTS (SELECT 1 FROM [MenuItems] WHERE [Id] = 15)
                    INSERT INTO [MenuItems] ([Id],[Icon],[IsActive],[MenuId],[Name],[Order],[RequiredPermission],[Route])
                    VALUES (15, 'salidas', 1, 3, 'Salidas', 4, 'salida.read', '/operaciones/salidas');
                SET IDENTITY_INSERT [MenuItems] OFF;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                -- Revertir: eliminar Salidas
                DELETE FROM [MenuItems] WHERE [Id] = 15;

                -- Restaurar Menus 4 y 5
                SET IDENTITY_INSERT [Menus] ON;
                INSERT INTO [Menus] ([Id],[Icon],[IsActive],[Name],[Order]) VALUES (4,'reservation',1,'Reserva',4);
                INSERT INTO [Menus] ([Id],[Icon],[IsActive],[Name],[Order]) VALUES (5,'venta',1,'Venta',5);
                SET IDENTITY_INSERT [Menus] OFF;

                -- Devolver ítems a sus menús originales
                UPDATE [MenuItems] SET [MenuId] = 4, [Order] = 1 WHERE [Id] = 8;
                UPDATE [MenuItems] SET [MenuId] = 5, [Order] = 1 WHERE [Id] = 10;

                -- Revertir nombre del Menu 3
                UPDATE [Menus] SET [Name] = 'Paquete', [Icon] = 'package' WHERE [Id] = 3;
            ");
        }
    }
}
