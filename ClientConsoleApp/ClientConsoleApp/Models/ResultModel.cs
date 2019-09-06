using Newtonsoft.Json.Linq;
using System.Net;

namespace ClientConsoleApp.Models
{
    public class ResultModel
    {
        public HttpStatusCode StatusCode { get; set; }

        public object Result { get; set; }
    }
}
