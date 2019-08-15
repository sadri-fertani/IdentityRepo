using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ApiApp.Models;

namespace ClientBackApi.Models.Rules
{
    public class AlphaNumericNameRule : IMonikerRule
    {
        private readonly IMonikerSettings _configuration;
        public string ErrorMessage => "Name must be alphanumeric.";

        public AlphaNumericNameRule(IMonikerSettings configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> CompliesWithRuleAsync(CampModel model)
        {
            if (!_configuration.CheckAlphaNumericName)
                return await Task.FromResult(true);

            return await Task.FromResult(new Regex("^[a-zA-Z0-9]*$").IsMatch(model.Name));
        }
    }
}
