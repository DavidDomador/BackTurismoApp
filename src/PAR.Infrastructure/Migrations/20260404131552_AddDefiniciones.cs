using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PAR.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDefiniciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Definicion",
                columns: table => new
                {
                    idDef = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dNombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    dDescripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    estado = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Definicion", x => x.idDef);
                });

            migrationBuilder.CreateTable(
                name: "DefinicionDetalle",
                columns: table => new
                {
                    idDefD = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ddValue = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    idDef = table.Column<int>(type: "int", nullable: false),
                    ddAbreviacion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ddDescripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    estado = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefinicionDetalle", x => x.idDefD);
                    table.ForeignKey(
                        name: "FK_DefinicionDetalle_Definicion_idDef",
                        column: x => x.idDef,
                        principalTable: "Definicion",
                        principalColumn: "idDef",
                        onDelete: ReferentialAction.Cascade);
                });

            // Seed data — tolerante a re-ejecución (usa IF NOT EXISTS + IDENTITY_INSERT)
            migrationBuilder.Sql(@"
                SET IDENTITY_INSERT [MenuItems] ON;
                IF NOT EXISTS (SELECT 1 FROM [MenuItems] WHERE [Id] = 5)
                    INSERT INTO [MenuItems] ([Id],[Icon],[IsActive],[MenuId],[Name],[Order],[RequiredPermission],[Route])
                    VALUES (5,'list',1,2,'Definiciones',2,'definicion.read','/config/definicion');
                SET IDENTITY_INSERT [MenuItems] OFF;

                SET IDENTITY_INSERT [Permissions] ON;
                IF NOT EXISTS (SELECT 1 FROM [Permissions] WHERE [Id] = 15)
                    INSERT INTO [Permissions] ([Id],[Description],[Module],[Name]) VALUES (15,'View definiciones','Definicion','definicion.read');
                IF NOT EXISTS (SELECT 1 FROM [Permissions] WHERE [Id] = 16)
                    INSERT INTO [Permissions] ([Id],[Description],[Module],[Name]) VALUES (16,'Create definiciones','Definicion','definicion.create');
                IF NOT EXISTS (SELECT 1 FROM [Permissions] WHERE [Id] = 17)
                    INSERT INTO [Permissions] ([Id],[Description],[Module],[Name]) VALUES (17,'Update definiciones','Definicion','definicion.update');
                IF NOT EXISTS (SELECT 1 FROM [Permissions] WHERE [Id] = 18)
                    INSERT INTO [Permissions] ([Id],[Description],[Module],[Name]) VALUES (18,'Delete definiciones','Definicion','definicion.delete');
                SET IDENTITY_INSERT [Permissions] OFF;

                IF NOT EXISTS (SELECT 1 FROM [RolePermissions] WHERE [RoleId]=1 AND [PermissionId]=15)
                    INSERT INTO [RolePermissions] ([RoleId],[PermissionId]) VALUES (1,15);
                IF NOT EXISTS (SELECT 1 FROM [RolePermissions] WHERE [RoleId]=1 AND [PermissionId]=16)
                    INSERT INTO [RolePermissions] ([RoleId],[PermissionId]) VALUES (1,16);
                IF NOT EXISTS (SELECT 1 FROM [RolePermissions] WHERE [RoleId]=1 AND [PermissionId]=17)
                    INSERT INTO [RolePermissions] ([RoleId],[PermissionId]) VALUES (1,17);
                IF NOT EXISTS (SELECT 1 FROM [RolePermissions] WHERE [RoleId]=1 AND [PermissionId]=18)
                    INSERT INTO [RolePermissions] ([RoleId],[PermissionId]) VALUES (1,18);
            ");

            migrationBuilder.CreateIndex(
                name: "IX_DefinicionDetalle_idDef",
                table: "DefinicionDetalle",
                column: "idDef");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DefinicionDetalle");

            migrationBuilder.DropTable(
                name: "Definicion");

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 15, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 16, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 17, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 18, 1 });

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 18);
        }
    }
}
