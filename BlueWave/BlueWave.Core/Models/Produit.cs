using System.ComponentModel.DataAnnotations;

namespace BlueWave.Core.Models
{
    public class Produit
    {
        [Key]
        public int CodeProduit { get; set; }

        [StringLength(150)]
        public string? NomProduit { get; set; }

        public int Prix { get; set; }
    }
}