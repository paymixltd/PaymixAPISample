using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymixSDK
{
    public static class Paths
    {
        /// <summary>
        /// Paymix AuthorizationServer 
        /// </summary>
        public const string AuthorizationServerBaseAddress = "http://auth-sandbox.veloxxa.com/";
        

        /// <summary>
        /// Paymix ResourceServer 
        /// </summary>
        
        public const string ResourceServerBaseAddress = "http://api-sandbox.veloxxa.com/";

        /// <summary>
        /// Your Merchant Web app should be running at this URL
        /// </summary>
        public const string AuthorizeCodeCallBackPath = "http://localhost:38500/";

        public const string AuthorizePath = "/oauth/authorize";
        public const string TokenPath = "/oauth/token";
        
        //public const string LoginPath = "/Account/Authorize";
        //public const string LogoutPath = "/Account/Logout";
        public const string MePath = "/Members/Profile";

        
    }
}