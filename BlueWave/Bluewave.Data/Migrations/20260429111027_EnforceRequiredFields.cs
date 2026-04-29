using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bluewave.Data.Migrations
{
    /// <inheritdoc />
    public partial class EnforceRequiredFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    RefClient = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NomClient = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PrenomClient = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telephone = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.RefClient);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Export",
                columns: table => new
                {
                    NumeroExport = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Delai = table.Column<int>(type: "int", nullable: false),
                    Statut = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Export", x => x.NumeroExport);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Fournisseur",
                columns: table => new
                {
                    RefFournisseur = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NomFournisseur = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PrenomsFournisseur = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TelephoneFournisseur = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fournisseur", x => x.RefFournisseur);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Stock",
                columns: table => new
                {
                    NumeroStock = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stock", x => x.NumeroStock);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Commande",
                columns: table => new
                {
                    NumeroCommande = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RefClient = table.Column<int>(type: "int", nullable: false),
                    CodeExport = table.Column<int>(type: "int", nullable: false),
                    DateCommande = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Destination = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commande", x => x.NumeroCommande);
                    table.ForeignKey(
                        name: "FK_Commande_Client_RefClient",
                        column: x => x.RefClient,
                        principalTable: "Client",
                        principalColumn: "RefClient",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Commande_Export_CodeExport",
                        column: x => x.CodeExport,
                        principalTable: "Export",
                        principalColumn: "NumeroExport",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Produit",
                columns: table => new
                {
                    CodeProduit = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NumeroStock = table.Column<int>(type: "int", nullable: false),
                    NomProduit = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Quantite = table.Column<int>(type: "int", nullable: false),
                    Date_reception = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Statut = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produit", x => x.CodeProduit);
                    table.ForeignKey(
                        name: "FK_Produit_Stock_NumeroStock",
                        column: x => x.NumeroStock,
                        principalTable: "Stock",
                        principalColumn: "NumeroStock",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Achat",
                columns: table => new
                {
                    IdAchat = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CodeProduit = table.Column<int>(type: "int", nullable: false),
                    NumeroCommande = table.Column<int>(type: "int", nullable: false),
                    Quantite = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Achat", x => x.IdAchat);
                    table.ForeignKey(
                        name: "FK_Achat_Commande_NumeroCommande",
                        column: x => x.NumeroCommande,
                        principalTable: "Commande",
                        principalColumn: "NumeroCommande",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Achat_Produit_CodeProduit",
                        column: x => x.CodeProduit,
                        principalTable: "Produit",
                        principalColumn: "CodeProduit",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Approvisionnement",
                columns: table => new
                {
                    IdApp = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RefFournisseur = table.Column<int>(type: "int", nullable: false),
                    CodeProduit = table.Column<int>(type: "int", nullable: false),
                    Certificat = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Approvisionnement", x => x.IdApp);
                    table.ForeignKey(
                        name: "FK_Approvisionnement_Fournisseur_RefFournisseur",
                        column: x => x.RefFournisseur,
                        principalTable: "Fournisseur",
                        principalColumn: "RefFournisseur",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Approvisionnement_Produit_CodeProduit",
                        column: x => x.CodeProduit,
                        principalTable: "Produit",
                        principalColumn: "CodeProduit",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Achat_CodeProduit",
                table: "Achat",
                column: "CodeProduit");

            migrationBuilder.CreateIndex(
                name: "IX_Achat_NumeroCommande",
                table: "Achat",
                column: "NumeroCommande");

            migrationBuilder.CreateIndex(
                name: "IX_Approvisionnement_CodeProduit",
                table: "Approvisionnement",
                column: "CodeProduit");

            migrationBuilder.CreateIndex(
                name: "IX_Approvisionnement_RefFournisseur",
                table: "Approvisionnement",
                column: "RefFournisseur");

            migrationBuilder.CreateIndex(
                name: "IX_Commande_CodeExport",
                table: "Commande",
                column: "CodeExport");

            migrationBuilder.CreateIndex(
                name: "IX_Commande_RefClient",
                table: "Commande",
                column: "RefClient");

            migrationBuilder.CreateIndex(
                name: "IX_Produit_NumeroStock",
                table: "Produit",
                column: "NumeroStock");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Achat");

            migrationBuilder.DropTable(
                name: "Approvisionnement");

            migrationBuilder.DropTable(
                name: "Commande");

            migrationBuilder.DropTable(
                name: "Fournisseur");

            migrationBuilder.DropTable(
                name: "Produit");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "Export");

            migrationBuilder.DropTable(
                name: "Stock");
        }
    }
}
