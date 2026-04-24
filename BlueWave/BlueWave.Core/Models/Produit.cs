using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
namespace BlueWave.Core.Models{
    public class Produit{

        [Key]
        public int CodeProduit {get; set;}
        public int NumeroStock {get; set;}

        [Required]
        [StringLength(150)]
        public required string NomProduit {get; set;}
        public int Quantite {get; set;}
        public DateTime Date_reception {get; set;}
        public bool Statut {get; set;}

        [ForeignKey(nameof(NumeroStock))]
        public virtual Stock Stock {get; set;} = null!;

        public virtual ICollection<Approvisionnement> Approvisionnement {get; set;} = new List<Approvisionnement>();

    }
}