using PaymixSDK;
using DotNetOpenAuth.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuthorizationCodeGrant.Service
{
    public static class PaymixResourceService
    {
        public static string URL { get; internal set; }

        public static WebServerClient GetClient()
        {
            return _getClient();
        }


        private static WebServerClient _getClient()
        {
            var resourceServerURI = new Uri(Paths.ResourceServerBaseAddress);
            var authorizationServer = new AuthorizationServerDescription
            {
                AuthorizationEndpoint = new Uri(resourceServerURI, Paths.AuthorizePath),
                TokenEndpoint = new Uri(resourceServerURI, Paths.TokenPath)
            };
            return new WebServerClient(authorizationServer, Clients.Client1.Id, Clients.Client1.Secret);

        }
    }
}