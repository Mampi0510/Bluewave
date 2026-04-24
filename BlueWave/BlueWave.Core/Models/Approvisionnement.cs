using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlueWave.Core.Models{
    public class Approvisionnement{
        [Key]
        public int IdApp {get; set;}
        public int RefFournisseur {get; set;}
        public int CodeProduit {get; set;}

        [Required]
        [StringLength(150)]
        public required string  Certificat {get; set;}

        [ForeignKey(nameof(RefFournisseur))]
        public virtual Fournisseur Fournisseur {get; set;} = null!;

        [ForeignKey(nameof(CodeProduit))]
        public virtual Produit Produit {get; set;} = null!;
    }
    
}