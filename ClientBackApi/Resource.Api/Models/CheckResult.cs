using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resource.Api.Models
{
    public class CheckResult
    {
        public bool Passed { get; set; }
        public List<string> Errors { get; set; }
    }
}
