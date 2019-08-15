using ApiApp.Models;
using System.Threading.Tasks;

namespace ClientBackApi.Models.Rules
{
    public interface IMonikerRule
    {
        string ErrorMessage { get; }
        Task<bool> CompliesWithRuleAsync(CampModel model);
    }
}
