using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace BlueWave.Core.Models{
    public class Fournisseur{
        [Key]
        public int RefFournisseur {get; set;}

        [Required]
        [StringLength(150)]
        public required string NomFournisseur {get; set;}

        [StringLength(150)]
        public required string PrenomsFournisseur {get; set;}

        [Required]
        [StringLength(25)]
        public required string TelephoneFournisseur {get; set;} 

        public virtual ICollection<Approvisionnement> Approvisionnement {get; set;} = new List<Approvisionnement>();
    }
}