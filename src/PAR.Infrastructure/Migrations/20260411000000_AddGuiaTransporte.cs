using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814

namespace PAR.Infrastructure.Migrations
{
    public partial class AddGuiaTransporte : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Description", "Module", "Name" },
                values: new object[,]
                {
                    { 7,  "View guias",         "Guia",       "guia.read"         },
                    { 8,  "Create guias",        "Guia",       "guia.create"       },
                    { 9,  "Update guias",        "Guia",       "guia.update"       },
                    { 10, "Delete guias",        "Guia",       "guia.delete"       },
                    { 11, "View transporte",     "Transporte", "transporte.read"   },
                    { 12, "Create transporte",   "Transporte", "transporte.create" },
                    { 13, "Update transporte",   "Transporte", "transporte.update" },
                    { 14, "Delete transporte",   "Transporte", "transporte.delete" }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "RoleId", "PermissionId" },
                values: new object[,]
                {
                    { 1, 7  },
                    { 1, 8  },
                    { 1, 9  },
                    { 1, 10 },
                    { 1, 11 },
                    { 1, 12 },
                    { 1, 13 },
                    { 1, 14 }
                });

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "Icon", "IsActive", "MenuId", "Name", "Order", "RequiredPermission", "Route" },
                values: new object[,]
                {
                    { 3, "book",  true, 1, "Guia",       2, "guia.read",       "/admin/guia"       },
                    { 4, "truck", true, 1, "Transporte", 3, "transporte.read", "/admin/transporte" }
                });

            migrationBuilder.CreateTable(
                name: "Guia",
                columns: table => new
                {
                    iCodigoGuia      = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    gNombre          = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    gApellidos       = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    gDni             = table.Column<string>(type: "nvarchar(20)",  maxLength: 20,  nullable: false),
                    gCorreo          = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt        = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt        = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId  = table.Column<int>(type: "int", nullable: true),
                    UpdatedByUserId  = table.Column<int>(type: "int", nullable: true),
                    IsActive         = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guia", x => x.iCodigoGuia);
                });

            migrationBuilder.CreateTable(
                name: "Choferes",
                columns: table => new
                {
                    iCodigoChofer    = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    gNombre          = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    gApellidos       = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    gDni             = table.Column<string>(type: "nvarchar(20)",  maxLength: 20,  nullable: false),
                    gLicencia        = table.Column<string>(type: "nvarchar(50)",  maxLength: 50,  nullable: false),
                    gTelefono        = table.Column<string>(type: "nvarchar(20)",  maxLength: 20,  nullable: true),
                    CreatedAt        = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt        = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId  = table.Column<int>(type: "int", nullable: true),
                    UpdatedByUserId  = table.Column<int>(type: "int", nullable: true),
                    IsActive         = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Choferes", x => x.iCodigoChofer);
                });

            migrationBuilder.CreateTable(
                name: "Vehiculos",
                columns: table => new
                {
                    iCodigoVehiculo  = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    gPlaca           = table.Column<string>(type: "nvarchar(20)",  maxLength: 20,  nullable: false),
                    gMarca           = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    gModelo          = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    gAnio            = table.Column<int>(type: "int", nullable: true),
                    gColor           = table.Column<string>(type: "nvarchar(50)",  maxLength: 50,  nullable: true),
                    CreatedAt        = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt        = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId  = table.Column<int>(type: "int", nullable: true),
                    UpdatedByUserId  = table.Column<int>(type: "int", nullable: true),
                    IsActive         = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehiculos", x => x.iCodigoVehiculo);
                });

            migrationBuilder.CreateTable(
                name: "ChoferVehiculo",
                columns: table => new
                {
                    iCodigoChoferVehiculo = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    ChoferID             = table.Column<int>(type: "int", nullable: false),
                    VehiculoID           = table.Column<int>(type: "int", nullable: false),
                    FechaAsignacion      = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive             = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt            = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId      = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChoferVehiculo", x => x.iCodigoChoferVehiculo);
                    table.ForeignKey(
                        name: "FK_ChoferVehiculo_Choferes_ChoferID",
                        column: x => x.ChoferID,
                        principalTable: "Choferes",
                        principalColumn: "iCodigoChofer",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChoferVehiculo_Vehiculos_VehiculoID",
                        column: x => x.VehiculoID,
                        principalTable: "Vehiculos",
                        principalColumn: "iCodigoVehiculo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChoferVehiculo_ChoferID",
                table: "ChoferVehiculo",
                column: "ChoferID");

            migrationBuilder.CreateIndex(
                name: "IX_ChoferVehiculo_VehiculoID",
                table: "ChoferVehiculo",
                column: "VehiculoID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "ChoferVehiculo");
            migrationBuilder.DropTable(name: "Choferes");
            migrationBuilder.DropTable(name: "Vehiculos");
            migrationBuilder.DropTable(name: "Guia");

            migrationBuilder.DeleteData(table: "MenuItems", keyColumn: "Id", keyValue: 3);
            migrationBuilder.DeleteData(table: "MenuItems", keyColumn: "Id", keyValue: 4);

            migrationBuilder.DeleteData(table: "RolePermissions", keyColumns: new[] { "RoleId", "PermissionId" }, keyValues: new object[] { 1, 7  });
            migrationBuilder.DeleteData(table: "RolePermissions", keyColumns: new[] { "RoleId", "PermissionId" }, keyValues: new object[] { 1, 8  });
            migrationBuilder.DeleteData(table: "RolePermissions", keyColumns: new[] { "RoleId", "PermissionId" }, keyValues: new object[] { 1, 9  });
            migrationBuilder.DeleteData(table: "RolePermissions", keyColumns: new[] { "RoleId", "PermissionId" }, keyValues: new object[] { 1, 10 });
            migrationBuilder.DeleteData(table: "RolePermissions", keyColumns: new[] { "RoleId", "PermissionId" }, keyValues: new object[] { 1, 11 });
            migrationBuilder.DeleteData(table: "RolePermissions", keyColumns: new[] { "RoleId", "PermissionId" }, keyValues: new object[] { 1, 12 });
            migrationBuilder.DeleteData(table: "RolePermissions", keyColumns: new[] { "RoleId", "PermissionId" }, keyValues: new object[] { 1, 13 });
            migrationBuilder.DeleteData(table: "RolePermissions", keyColumns: new[] { "RoleId", "PermissionId" }, keyValues: new object[] { 1, 14 });

            migrationBuilder.DeleteData(table: "Permissions", keyColumn: "Id", keyValue: 7);
            migrationBuilder.DeleteData(table: "Permissions", keyColumn: "Id", keyValue: 8);
            migrationBuilder.DeleteData(table: "Permissions", keyColumn: "Id", keyValue: 9);
            migrationBuilder.DeleteData(table: "Permissions", keyColumn: "Id", keyValue: 10);
            migrationBuilder.DeleteData(table: "Permissions", keyColumn: "Id", keyValue: 11);
            migrationBuilder.DeleteData(table: "Permissions", keyColumn: "Id", keyValue: 12);
            migrationBuilder.DeleteData(table: "Permissions", keyColumn: "Id", keyValue: 13);
            migrationBuilder.DeleteData(table: "Permissions", keyColumn: "Id", keyValue: 14);
        }
    }
}
