using System;
using System.ComponentModel.DataAnnotations;

namespace ApiApp.Models
{
    public class PaysModel
    {
        [Required]
        public Int16 Id { get; set; }

        [Required]
        public int Code { get; set; }

        [Required]
        [StringLength(2, MinimumLength = 2)]
        public string Alpha2 { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string Alpha3 { get; set; }

        [Required]
        [StringLength(45, MinimumLength = 4)]
        public string NomEN { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 4)]
        public string NomFR { get; set; }
    }
}
