using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCoreConsole.Migrations
{
    public partial class EFCoreConsoleDataMachineContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MachineType",
                columns: table => new
                {
                    MachineTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(unicode: false, maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineType", x => x.MachineTypeID);
                });

            migrationBuilder.CreateTable(
                name: "OperatingSys",
                columns: table => new
                {
                    OperatingSysID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(unicode: false, maxLength: 35, nullable: false),
                    StillSupported = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperatingSys", x => x.OperatingSysID);
                });

            migrationBuilder.CreateTable(
                name: "WarrantyProvider",
                columns: table => new
                {
                    WarrantyProviderID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProviderName = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    SupportExtension = table.Column<int>(nullable: true),
                    SupportNumber = table.Column<string>(unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarrantyProvider", x => x.WarrantyProviderID);
                });

            migrationBuilder.CreateTable(
                name: "Machine",
                columns: table => new
                {
                    MachineID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(unicode: false, maxLength: 25, nullable: false),
                    GeneralRole = table.Column<string>(unicode: false, maxLength: 25, nullable: false),
                    InstalledRoles = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    OperatingSysID = table.Column<int>(nullable: false),
                    MachineTypeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Machine", x => x.MachineID);
                    table.ForeignKey(
                        name: "FK_MachineType",
                        column: x => x.MachineTypeID,
                        principalTable: "MachineType",
                        principalColumn: "MachineTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OperatingSys",
                        column: x => x.OperatingSysID,
                        principalTable: "OperatingSys",
                        principalColumn: "OperatingSysID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MachineWarranty",
                columns: table => new
                {
                    MachineWarrantyID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ServiceTag = table.Column<string>(unicode: false, maxLength: 20, nullable: false),
                    WarrantyExpiration = table.Column<DateTime>(type: "date", nullable: false),
                    MachineID = table.Column<int>(nullable: false),
                    WarrantyProviderID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineWarranty", x => x.MachineWarrantyID);
                    table.ForeignKey(
                        name: "FK_WarrantyProvider",
                        column: x => x.WarrantyProviderID,
                        principalTable: "WarrantyProvider",
                        principalColumn: "WarrantyProviderID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SupportTicket",
                columns: table => new
                {
                    SupportTicketID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateReported = table.Column<DateTime>(type: "date", nullable: false),
                    DateResolved = table.Column<DateTime>(type: "date", nullable: true),
                    IssueDescription = table.Column<string>(unicode: false, maxLength: 150, nullable: false),
                    IssueDetail = table.Column<string>(unicode: false, nullable: true),
                    TicketOpenedBy = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    MachineID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportTicket", x => x.SupportTicketID);
                    table.ForeignKey(
                        name: "FK_Machine",
                        column: x => x.MachineID,
                        principalTable: "Machine",
                        principalColumn: "MachineID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SupportLog",
                columns: table => new
                {
                    SupportLogID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SupportLogEntryDate = table.Column<DateTime>(type: "date", nullable: false),
                    SupportLogEntry = table.Column<string>(unicode: false, nullable: false),
                    SupportLogUpdatedBy = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    SupportTicketID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportLog", x => x.SupportLogID);
                    table.ForeignKey(
                        name: "FK_SupportTicket",
                        column: x => x.SupportTicketID,
                        principalTable: "SupportTicket",
                        principalColumn: "SupportTicketID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Machine_MachineTypeID",
                table: "Machine",
                column: "MachineTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Machine_OperatingSysID",
                table: "Machine",
                column: "OperatingSysID");

            migrationBuilder.CreateIndex(
                name: "IX_MachineWarranty_WarrantyProviderID",
                table: "MachineWarranty",
                column: "WarrantyProviderID");

            migrationBuilder.CreateIndex(
                name: "IX_SupportLog_SupportTicketID",
                table: "SupportLog",
                column: "SupportTicketID");

            migrationBuilder.CreateIndex(
                name: "IX_SupportTicket_MachineID",
                table: "SupportTicket",
                column: "MachineID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MachineWarranty");

            migrationBuilder.DropTable(
                name: "SupportLog");

            migrationBuilder.DropTable(
                name: "WarrantyProvider");

            migrationBuilder.DropTable(
                name: "SupportTicket");

            migrationBuilder.DropTable(
                name: "Machine");

            migrationBuilder.DropTable(
                name: "MachineType");

            migrationBuilder.DropTable(
                name: "OperatingSys");
        }
    }
}
