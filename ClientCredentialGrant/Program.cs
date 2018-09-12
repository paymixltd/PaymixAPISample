using PaymixSDK;
using DotNetOpenAuth.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace ClientCredentialGrant
{
    class Program
    {
        private static WebServerClient _webServerClient;
        private static string _accessToken;

        static void Main(string[] args)
        {
            InitializeWebServerClient();

            Console.WriteLine("Requesting Token...");
            RequestToken();

            Console.WriteLine("Access Token: {0}", _accessToken);

            Console.WriteLine("Access Protected Resource");
            AccessProtectedResource();
        }

        private static void InitializeWebServerClient()
        {
             var authorizationServerUri = new Uri(Paths.AuthorizationServerBaseAddress);
            var authorizationServer = new AuthorizationServerDescription
            {
                AuthorizationEndpoint = new Uri(authorizationServerUri, Paths.AuthorizePath),
                TokenEndpoint = new Uri(authorizationServerUri, Paths.TokenPath)
            };
            _webServerClient = new WebServerClient(authorizationServer, "INNOVERTU", "sRXRYaybQGtUpQ9W");
        }

        private static void RequestToken()
        {
            var state = _webServerClient.GetClientAccessToken(new[] { "TRUSTED"});
            _accessToken = state.AccessToken;
        }

        private static void AccessProtectedResource()
        {
            var resourceServerUri = new Uri(Paths.ResourceServerBaseAddress);
            var client = new HttpClient(_webServerClient.CreateAuthorizingHandler(_accessToken));
            
            //Uri(resourceServerUri, "/PaymixWS_Resource/Cobrander/Customer/Application")

            using (StreamReader file = File.OpenText(@"C:\Users\sm\Desktop\APPMESSAGE.JSON"))
            {
                string jsonApplication = file.ReadToEnd();
                HttpContent myContent = new StringContent(jsonApplication, Encoding.UTF8,
                                   "application/json");
                var response = client.PostAsync(new Uri(resourceServerUri, "/PaymixWS_Resource/Cobrander/Customer/Application"), myContent);
                var contents = response.Result.Content.ReadAsStringAsync();
            }

            
            //var retVal = JObject.Parse(contents);
            
            //Console.WriteLine(body);
         }
    }
}