using DotNetOpenAuth.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PaymixSDK;

namespace AuthorizationCodeGrant.Service
{
    
    
    public static class PaymixAuthService
    {
        public static string URL {

            get
            {
                return Paths.ResourceServerBaseAddress;
            }
        }

        public static WebServerClient GetClient()
        {
            return _getClient();
        }


        private static WebServerClient _getClient()
        {
            var authorizationServerUri = new Uri(Paths.AuthorizationServerBaseAddress);
            var authorizationServer = new AuthorizationServerDescription
            {
                AuthorizationEndpoint = new Uri(authorizationServerUri, Paths.AuthorizePath),
                TokenEndpoint = new Uri(authorizationServerUri, Paths.TokenPath)
            };
            return new WebServerClient(authorizationServer, Clients.Client1.Id, Clients.Client1.Secret);

        }

        
   
    }


    



}