using System;
using System.Threading.Tasks;
using ApiApp.Models;

namespace ClientBackApi.Models.Rules
{
    public class UpperCaseNameRule : IMonikerRule
    {
        private readonly IMonikerSettings _configuration;
        public string ErrorMessage => "Name must respect uppercase format.";

        public UpperCaseNameRule(IMonikerSettings configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> CompliesWithRuleAsync(CampModel model)
        {
            if (!_configuration.CheckUpperCaseName)
                return await Task.FromResult(true);

            return await Task.FromResult(Char.IsUpper(model.Name, 0));
        }
    }
}
