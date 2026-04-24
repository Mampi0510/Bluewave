using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace BlueWave.Core.Models{
    public class Export{

        [Key]
        public int NumeroExport {get; set;}

        [Required]
        public int Delai{get; set;}

        [Required]
        public string Statut { get; set; } = "";

        public virtual ICollection<Commande> Commande {get; set;} = new List<Commande>();
    }
}