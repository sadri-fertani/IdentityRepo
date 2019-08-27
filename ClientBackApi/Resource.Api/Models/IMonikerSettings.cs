namespace Resource.Api.Models
{
    public interface IMonikerSettings
    {
        bool PutEnabled { get; set; }
        bool DeleteEnabled { get; set; }
        bool CheckUpperCaseName { get; set; }
        bool CheckAlphaNumericName { get; set; }
    }
}
