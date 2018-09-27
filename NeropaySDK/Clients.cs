using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymixSDK
{
    public static class Clients
    {
        public readonly static Client Client1 = new Client
        {
            Id = "pagatudo",
            Secret = "pagatudo",
            RedirectUrl = Paths.AuthorizeCodeCallBackPath
        };

        
    }

    public class Client
    {
        public string Id { get; set; }
        public string Secret { get; set; }
        public string RedirectUrl { get; set; }
    }
}