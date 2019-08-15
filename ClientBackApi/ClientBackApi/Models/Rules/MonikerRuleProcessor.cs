using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiApp.Models;

namespace ClientBackApi.Models.Rules
{
    public class MonikerRuleProcessor : IMonikerRuleProcessor
    {
        private readonly IEnumerable<IMonikerRule> _rules;

        public MonikerRuleProcessor(IEnumerable<IMonikerRule> rules)
        {
            _rules = rules;
        }

        public async Task<CheckResult> PassesAllRulesAsync(CampModel model)
        {
            var passedRules = true;
            var errors = new List<string>();

            foreach (var rule in _rules)
            {
                if (!await rule.CompliesWithRuleAsync(model))
                {
                    passedRules = false;
                    errors.Add(rule.ErrorMessage);
                }
            }

            return new CheckResult
            {
                Passed = passedRules,
                Errors = errors
            };
        }
    }
}
