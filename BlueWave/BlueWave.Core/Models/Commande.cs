using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlueWave.Core.Models
{
    public class Commande
    {
        [Key]
        public int NumeroCommande { get; set; }
        public int RefClient { get; set; }
        public int Delai { get; set; }
        public DateTime DateCommande { get; set; }

        [Required]
        [StringLength(150)]
        public required string Destination { get; set; }

        [ForeignKey(nameof(RefClient))]
        public Client? Client { get; set; }

        public ICollection<Achat> Achats { get; set; } = new List<Achat>();
    }
}