using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using BlueWave.Core.Models;
using System.Diagnostics;
using System.IO;

namespace BlueWave.Services
{
    public static class FacturePdfService
    {
        public static void Generer(Commande commande)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var dossierFactures = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Factures");
            Directory.CreateDirectory(dossierFactures);
            var path = Path.Combine(dossierFactures, $"Facture_{commande.NumeroCommande}_{DateTime.Now:yyyyMMdd_HHmm}.pdf");

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(40);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    page.Header().Column(col =>
                    {
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("BLUEWAVE").FontSize(24).Bold().FontColor("#0D2137");
                                c.Item().Text("Gestion Commerciale").FontSize(12).FontColor("#607D8B");
                            });
                            row.ConstantItem(150).Column(c =>
                            {
                                c.Item().Text("FACTURE").FontSize(20).Bold().FontColor("#00B386").AlignRight();
                                c.Item().Text($"N° {commande.NumeroCommande}").FontSize(12).AlignRight();
                                c.Item().Text($"Date : {commande.DateCommande:dd/MM/yyyy}").FontSize(11).AlignRight();
                            });
                        });
                        col.Item().PaddingTop(10).LineHorizontal(1).LineColor("#E8ECF0");
                    });

                    page.Content().PaddingTop(20).Column(col =>
                    {
                        // Infos client
                        col.Item().Background("#F8FAFB").Padding(15).Column(c =>
                        {
                            c.Item().Text("INFORMATIONS CLIENT").FontSize(10).Bold().FontColor("#607D8B");
                            c.Item().PaddingTop(5).Text($"Client : {commande.Client?.NomClient} {commande.Client?.PrenomClient}").Bold();
                            c.Item().Text($"Destination : {commande.Destination}");
                            c.Item().Text($"Délai : {commande.Delai} jours");
                        });

                        col.Item().PaddingTop(20).Text("DÉTAIL DE LA COMMANDE").FontSize(12).Bold().FontColor("#0D2137");
                        col.Item().PaddingTop(10).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(40);
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1);
                            });

                            // Header
                            table.Header(header =>
                            {
                                header.Cell().Background("#0D2137").Padding(8)
                                    .Text("#").FontColor(Colors.White).Bold();
                                header.Cell().Background("#0D2137").Padding(8)
                                    .Text("Produit").FontColor(Colors.White).Bold();
                                header.Cell().Background("#0D2137").Padding(8)
                                    .Text("Quantité").FontColor(Colors.White).Bold();
                                header.Cell().Background("#0D2137").Padding(8)
                                    .Text("Prix unit.").FontColor(Colors.White).Bold();
                                header.Cell().Background("#0D2137").Padding(8)
                                    .Text("Total").FontColor(Colors.White).Bold();
                            });

                            // Lignes
                            int i = 1;
                            int total = 0;
                            foreach (var achat in commande.Achats ?? new List<Achat>())
                            {
                                string bg = i % 2 == 0 ? "#F8FAFB" : "#FFFFFF";
                                var sous_total = achat.Quantite * (achat.Produit?.Prix ?? 0);
                                total += sous_total;

                                table.Cell().Background(bg).Padding(8).Text(i.ToString());
                                table.Cell().Background(bg).Padding(8).Text(achat.Produit?.NomProduit ?? "-");
                                table.Cell().Background(bg).Padding(8).Text(achat.Quantite.ToString());
                                table.Cell().Background(bg).Padding(8).Text($"{achat.Produit?.Prix ?? 0} Ar");
                                table.Cell().Background(bg).Padding(8).Text($"{sous_total} Ar");
                                i++;
                            }

                            // Total
                            table.Cell().ColumnSpan(4).Background("#E8F5E9").Padding(8)
                                .Text("TOTAL").Bold().AlignRight();
                            table.Cell().Background("#E8F5E9").Padding(8)
                                .Text($"{total} Ar").Bold().FontColor("#00B386");
                        });
                    });

                    page.Footer().AlignCenter()
                        .Text($"BlueWave — Facture générée le {DateTime.Now:dd/MM/yyyy à HH:mm}")
                        .FontSize(9).FontColor("#9E9E9E");
                });
            }).GeneratePdf(path);

            // Ouvrir le PDF
            Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
        }
    }
}