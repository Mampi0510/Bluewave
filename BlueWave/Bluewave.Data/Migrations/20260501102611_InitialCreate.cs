using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bluewave.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CLIENT",
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
                    table.PrimaryKey("PK_CLIENT", x => x.RefClient);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EXPORT",
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
                    table.PrimaryKey("PK_EXPORT", x => x.NumeroExport);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FOURNISSEUR",
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
                    table.PrimaryKey("PK_FOURNISSEUR", x => x.RefFournisseur);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "STOCK",
                columns: table => new
                {
                    NumeroStock = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NomStock = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Quantite = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STOCK", x => x.NumeroStock);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "COMMANDE",
                columns: table => new
                {
                    NumeroCommande = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RefClient = table.Column<int>(type: "int", nullable: false),
                    NumeroExport = table.Column<int>(type: "int", nullable: false),
                    DateCommande = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Destination = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COMMANDE", x => x.NumeroCommande);
                    table.ForeignKey(
                        name: "FK_COMMANDE_CLIENT_RefClient",
                        column: x => x.RefClient,
                        principalTable: "CLIENT",
                        principalColumn: "RefClient",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_COMMANDE_EXPORT_NumeroExport",
                        column: x => x.NumeroExport,
                        principalTable: "EXPORT",
                        principalColumn: "NumeroExport",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PRODUIT",
                columns: table => new
                {
                    CodeProduit = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NomProduit = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Prix = table.Column<int>(type: "int", nullable: false),
                    Statut = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    StockNumeroStock = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUIT", x => x.CodeProduit);
                    table.ForeignKey(
                        name: "FK_PRODUIT_STOCK_StockNumeroStock",
                        column: x => x.StockNumeroStock,
                        principalTable: "STOCK",
                        principalColumn: "NumeroStock");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ACHAT",
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
                    table.PrimaryKey("PK_ACHAT", x => x.IdAchat);
                    table.ForeignKey(
                        name: "FK_ACHAT_COMMANDE_NumeroCommande",
                        column: x => x.NumeroCommande,
                        principalTable: "COMMANDE",
                        principalColumn: "NumeroCommande",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ACHAT_PRODUIT_CodeProduit",
                        column: x => x.CodeProduit,
                        principalTable: "PRODUIT",
                        principalColumn: "CodeProduit",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "APPROVISIONNEMENT",
                columns: table => new
                {
                    IdApp = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RefFournisseur = table.Column<int>(type: "int", nullable: false),
                    CodeProduit = table.Column<int>(type: "int", nullable: false),
                    NumeroStock = table.Column<int>(type: "int", nullable: false),
                    Quantite = table.Column<int>(type: "int", nullable: false),
                    DateReception = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Certificat = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APPROVISIONNEMENT", x => x.IdApp);
                    table.ForeignKey(
                        name: "FK_APPROVISIONNEMENT_FOURNISSEUR_RefFournisseur",
                        column: x => x.RefFournisseur,
                        principalTable: "FOURNISSEUR",
                        principalColumn: "RefFournisseur",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_APPROVISIONNEMENT_PRODUIT_CodeProduit",
                        column: x => x.CodeProduit,
                        principalTable: "PRODUIT",
                        principalColumn: "CodeProduit",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_APPROVISIONNEMENT_STOCK_NumeroStock",
                        column: x => x.NumeroStock,
                        principalTable: "STOCK",
                        principalColumn: "NumeroStock",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ACHAT_CodeProduit",
                table: "ACHAT",
                column: "CodeProduit");

            migrationBuilder.CreateIndex(
                name: "IX_ACHAT_NumeroCommande",
                table: "ACHAT",
                column: "NumeroCommande");

            migrationBuilder.CreateIndex(
                name: "IX_APPROVISIONNEMENT_CodeProduit",
                table: "APPROVISIONNEMENT",
                column: "CodeProduit");

            migrationBuilder.CreateIndex(
                name: "IX_APPROVISIONNEMENT_NumeroStock",
                table: "APPROVISIONNEMENT",
                column: "NumeroStock");

            migrationBuilder.CreateIndex(
                name: "IX_APPROVISIONNEMENT_RefFournisseur",
                table: "APPROVISIONNEMENT",
                column: "RefFournisseur");

            migrationBuilder.CreateIndex(
                name: "IX_COMMANDE_NumeroExport",
                table: "COMMANDE",
                column: "NumeroExport");

            migrationBuilder.CreateIndex(
                name: "IX_COMMANDE_RefClient",
                table: "COMMANDE",
                column: "RefClient");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUIT_StockNumeroStock",
                table: "PRODUIT",
                column: "StockNumeroStock");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ACHAT");

            migrationBuilder.DropTable(
                name: "APPROVISIONNEMENT");

            migrationBuilder.DropTable(
                name: "COMMANDE");

            migrationBuilder.DropTable(
                name: "FOURNISSEUR");

            migrationBuilder.DropTable(
                name: "PRODUIT");

            migrationBuilder.DropTable(
                name: "CLIENT");

            migrationBuilder.DropTable(
                name: "EXPORT");

            migrationBuilder.DropTable(
                name: "STOCK");
        }
    }
}
