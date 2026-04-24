using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace BlueWave.Core.Models{
    public class Stock{

        [Key]
        public int NumeroStock {get; set;}
        
        [Required]
        [StringLength(25)]
        public required string Type {get; set;}

        public virtual ICollection<Produit> Produid {get; set;} = new List<Produit>();
    }
}