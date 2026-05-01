using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlueWave.Core.Models
{
    public class Approvisionnement
    {
        [Key]
        public int IdApp { get; set; }

        public int RefFournisseur { get; set; }
        public int CodeProduit { get; set; }
        public int NumeroStock { get; set; }
        public int Quantite { get; set; }
        public DateTime DateReception { get; set; }

        [Required]
        [StringLength(150)]
        public required string Certificat { get; set; }

        [ForeignKey(nameof(RefFournisseur))]
        public Fournisseur? Fournisseur { get; set; }

        [ForeignKey(nameof(CodeProduit))]
        public Produit? Produit { get; set; }

        [ForeignKey(nameof(NumeroStock))]
        public Stock? Stock { get; set; }
    }
}