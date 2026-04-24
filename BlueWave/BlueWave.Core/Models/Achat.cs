using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlueWave.Core.Models{
    public class Achat{
    
    [Key]
    public int IdAchat {get; set;}
    public int  CodeProduit{ get; set; }

    public int NumeroCommande {get; set;} 

    public int Quantite {get; set;}

    [ForeignKey(nameof(CodeProduit))]
    public virtual Produit Produit {get; set;} = null!;

    [ForeignKey(nameof(NumeroCommande))]
    public virtual Commande Commande {get; set;} = null!;

  }
}

