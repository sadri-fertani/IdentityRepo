using System.Threading.Tasks;

namespace Resource.Api.Models.Rules
{
    public interface IMonikerRule
    {
        string ErrorMessage { get; }
        Task<bool> CompliesWithRuleAsync(CampModel model);
    }
}
