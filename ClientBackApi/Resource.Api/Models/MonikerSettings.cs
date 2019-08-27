namespace Resource.Api.Models
{
    public class MonikerSettings : IMonikerSettings
    {
        public bool PutEnabled { get; set; }
        public bool DeleteEnabled { get; set; }
        public bool CheckUpperCaseName { get; set; }
        public bool CheckAlphaNumericName { get; set; }
    }
}
