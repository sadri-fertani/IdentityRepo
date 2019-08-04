using System.ComponentModel.DataAnnotations.Schema;

namespace ApiApp.Data
{
    [Table("Pays", Schema = "ref")]
    public class Pays : Reference
    {
        public int Code { get; set; }
        public string Alpha2 { get; set; }
        public string Alpha3 { get; set; }
        public string NomEN { get; set; }
        public string NomFR { get; set; }
    }
}