using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace  BlueWave.Core.Models{
    public class Client{
        [Key]
        public int RefClient {get; set;}
        
        [Required]
        [StringLength(150)]
        public required string NomClient {get; set;}
        
        [StringLength(150)]
        public required string PrenomClient {get; set;}

        [Required]
        [StringLength(25)]
        public required string Telephone {get; set;}
        public virtual ICollection<Commande> Commande {get; set;} = new List<Commande>();
    }

}