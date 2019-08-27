using System.Threading.Tasks;

namespace Resource.Api.Models.Rules
{
    public interface IMonikerRuleProcessor
    {
        Task<CheckResult> PassesAllRulesAsync(CampModel model);
    }
}
