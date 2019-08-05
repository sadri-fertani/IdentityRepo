using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiApp.Data
{
    [Serializable]
    [Table("Pays", Schema = "ref")]
    public class Pays : Reference, ICloneable
    {
        public int Code { get; set; }
        public string Alpha2 { get; set; }
        public string Alpha3 { get; set; }
        public string NomEN { get; set; }
        public string NomFR { get; set; }

        public object Clone()
        {
            return new Pays
            {
                Id = this.Id,
                Code = this.Code,
                Alpha2 = this.Alpha2,
                Alpha3 = this.Alpha3,
                NomEN = this.NomEN,
                NomFR = this.NomFR
            };
        }

        public override bool Equals(object obj)
        {
            Pays extObj = obj as Pays;

            return extObj == null ?
                false : (extObj.Code.Equals(this.Code) && extObj.Alpha2.Equals(this.Alpha2) && extObj.Alpha3.Equals(this.Alpha3));
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}