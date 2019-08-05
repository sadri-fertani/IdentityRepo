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

        public override bool Equals(object obj)
        {
            PaysModel extObj = obj as PaysModel;

            return extObj == null ?
                false : (extObj.Code.Equals(this.Code) && extObj.Alpha2.Equals(this.Alpha2) && extObj.Alpha3.Equals(this.Alpha3));
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
