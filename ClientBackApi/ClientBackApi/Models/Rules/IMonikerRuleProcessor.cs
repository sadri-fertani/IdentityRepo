using ApiApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClientBackApi.Models.Rules
{
    public interface IMonikerRuleProcessor
    {
        Task<CheckResult> PassesAllRulesAsync(CampModel model);
    }
}
