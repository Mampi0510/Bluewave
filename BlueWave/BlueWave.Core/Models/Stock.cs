using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace BlueWave.Core.Models
{
    public class Stock
    {
        [Key]
        public int NumeroStock { get; set; }

        [Required]
        public string NomStock { get; set; } = string.Empty;

        public int Quantite { get; set; }
    }
}