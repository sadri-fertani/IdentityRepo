using System;
using System.ComponentModel.DataAnnotations;

namespace ClientConsoleApp.Models
{
    public class PaysModel
    {
        public Int16 Id { get; set; }
        public int Code { get; set; }
        public string Alpha2 { get; set; }
        public string Alpha3 { get; set; }
        public string NomEN { get; set; }
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
