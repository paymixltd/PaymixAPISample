using PaymixSDK;
using DotNetOpenAuth.OAuth2;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AuthorizationCodeGrant.Service;

namespace ResourceOwnerPasswordCredentialGrant
{
    class Program
    {
        private static WebServerClient _webServerClient;
        private static string _accessToken;
        private static Uri resourceServerUri = new Uri(Paths.ResourceServerBaseAddress);

        static void Main(string[] args)
        {
            AsyncMain().Wait();
        }

        private static async Task AsyncMain()
        {
            InitializeWebServerClient();

            Console.WriteLine("Requesting Token...");
            RequestToken();

            Console.WriteLine("Access Token: {0}", _accessToken);

            //ChangePassword();

            //Signup();


            NewUserMobile();

            // var c = await UpgradeCDD(); OTP ISSUE -- BROKEN 

            //var c = await EWalletTransfer();

             var c = await GetAccountSummary(); //OK

            // var a = await GetAccountBalance("20210100001"); OK



            // var x = await EWalletTransfer("savasmanyasli@mailinator.com",100); -- BROKEN

            // var m = await SendFundsToMerchant("NRP", 10); -- BROKEN
            //var t = await GetUserProfile();

            //var s = await SetUserProfile(t);  // 

            //s = await GetUserProfile();

            var d = await GetDocuments();

            var x = await GetMetadata();

        }

        private static void NewUserMobile()
        {
            Signup();
            
        }

        private static async Task<JObject> GetMetadata()
        {
            var client = new HttpClient(_webServerClient.CreateAuthorizingHandler(_accessToken));
            client.DefaultRequestHeaders.Accept
            .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                var response = await client.GetAsync(new Uri(resourceServerUri, "/PagatudoAPI/Members/Metadata?name=documentType&lang=en"));
                var contents = await response.Content.ReadAsStringAsync();
                var retVal = JObject.Parse(contents);
                return retVal;
            }
            catch (Exception ex)
            {
                throw;
            }


        }

        private static async Task<JObject> SetUserProfile(JObject t)
        {
            var client = new HttpClient(_webServerClient.CreateAuthorizingHandler(_accessToken));
            client.DefaultRequestHeaders.Accept
            .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            dynamic content = t["responseBody"][0];
            content.dateOfBirth = "27-11-1979";
            content.cityOfBirth = "Istanbul2";
            content.mobilePrefix = "ITA";
            content.mobile = "99999958";
            content.title = "Dr";
            content.gender = "F";
            content.documentExpireDate = "29-05-2018";
            HttpContent myContent = new StringContent(t["responseBody"][0].ToString(), Encoding.UTF8,
                                    "application/json");
            try
            {
                var response = await client.PostAsync(new Uri(resourceServerUri, Paths.MePath), myContent);
                var contents = await response.Content.ReadAsStringAsync();
                var retVal = JObject.Parse(contents);
                return retVal;
            }
            catch (Exception ex)
            {
                throw;
            }


        }


        private static async Task<JObject> SendDocuments(JObject t)
        {
            var client = new HttpClient(_webServerClient.CreateAuthorizingHandler(_accessToken));
            client.DefaultRequestHeaders.Accept
            .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            dynamic content = t["responseBody"][0];
            HttpContent myContent = new StringContent(t["responseBody"][0].ToString(), Encoding.UTF8,
                                    "application/json");
            try
            {
                var response = await client.PutAsync(new Uri(resourceServerUri, Paths.MePath), myContent);
                var contents = await response.Content.ReadAsStringAsync();
                var retVal = JObject.Parse(contents);
                return retVal;
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        private static async Task<JObject> GetDocuments()
        {
            var client = new HttpClient(_webServerClient.CreateAuthorizingHandler(_accessToken));
            client.DefaultRequestHeaders.Accept
            .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                var response = await client.GetAsync(new Uri(resourceServerUri, "/PagatudoAPI/Members/Document/Info"));
                var contents = await response.Content.ReadAsStringAsync();
                var retVal = JObject.Parse(contents);
                return retVal;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private static async Task<JObject> GetAccountSummary()
        {

            
            var client = new HttpClient(_webServerClient.CreateAuthorizingHandler(_accessToken));
            client.DefaultRequestHeaders.Accept
            .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            dynamic content = new JObject();
            HttpContent myContent = new StringContent(content.ToString(), Encoding.UTF8,
                                    "application/json");
            try
            {
                var response = await client.GetAsync(new Uri(resourceServerUri, "/PagatudoAPI/Members/Account"));
                var contents = await response.Content.ReadAsStringAsync();
                return JObject.Parse(contents);
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        private static async Task<JObject> GetAccountBalance(string AccountNumber)
        {


            var client = new HttpClient(_webServerClient.CreateAuthorizingHandler(_accessToken));
            client.DefaultRequestHeaders.Accept
            .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            dynamic content = new JObject();
            HttpContent myContent = new StringContent(content.ToString(), Encoding.UTF8,
                                    "application/json");
            try
            {
                var response = await client.GetAsync(new Uri(resourceServerUri, "/PagatudoAPI/Members/Account/Balance?accountNumber=" + AccountNumber));
                var contents = await response.Content.ReadAsStringAsync();
                return JObject.Parse(contents);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private static async Task<string> EWalletTransfer(string UserName , decimal Amount)
        {

            var client = new HttpClient(_webServerClient.CreateAuthorizingHandler(_accessToken));
            client.DefaultRequestHeaders.Accept
            .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            dynamic content = new JObject();
            content.customerUserName = UserName;
            content.amount = Amount;

            HttpContent myContent = new StringContent(content.ToString(), Encoding.UTF8,
                                    "application/json");
            

            try
            {
                var response = await client.PostAsync(new Uri(resourceServerUri, "/PagatudoAPI/Members/Transfer/Ewallet"), myContent);
                var contents = await response.Content.ReadAsStringAsync();
                return contents;
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        private static async Task<string> SendFundsToMerchant(string merchantCode, decimal Amount)
        {

            var client = new HttpClient(_webServerClient.CreateAuthorizingHandler(_accessToken));
            client.DefaultRequestHeaders.Accept
            .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            dynamic content = new JObject();
            content.merchantCode = merchantCode;
            content.amount = Amount;

            HttpContent myContent = new StringContent(content.ToString(), Encoding.UTF8,
                                    "application/json");
            try
            {
                var response = await client.PostAsync(new Uri(resourceServerUri, "/PagatudoAPI/Members/Transfer/Merchant"), myContent);
                var contents = await response.Content.ReadAsStringAsync();
                return contents;
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        private static async Task<String> UpgradeCDD()
        {
            var client = new HttpClient(_webServerClient.CreateAuthorizingHandler(_accessToken));
            client.DefaultRequestHeaders.Accept
            .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            dynamic content = new JObject();
            HttpContent myContent = new StringContent(content.ToString(), Encoding.UTF8,
                                    "application/json");
            try
            {
                var response = await client.PostAsync(new Uri(resourceServerUri, "/PagatudoAPI/Members/Upgrade"),myContent);
                var contents = await response.Content.ReadAsStringAsync();
                return contents;
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        private static void InitializeWebServerClient()
        {
            var authorizationServerUri = new Uri(Paths.AuthorizationServerBaseAddress);
            var authorizationServer = new AuthorizationServerDescription
            {
                AuthorizationEndpoint = new Uri(authorizationServerUri, Paths.AuthorizePath),
                TokenEndpoint = new Uri(authorizationServerUri, Paths.TokenPath)
            };
            _webServerClient = new WebServerClient(authorizationServer, Clients.Client1.Id, Clients.Client1.Secret);
        }

        private static void RequestToken()
        {
            var state = _webServerClient.ExchangeUserCredentialForToken("savasmanyasli@mailinator.com", "Password!", scopes: new string[] { "USER_COMPLIANCE" , "USER_FINANCIAL" , "USER_REGISTRATION" , "PUBLIC_DATA" });
            
            _accessToken = state.AccessToken;
        }

        private async static Task  Signup()
        {
            var resourceServerUri = new Uri(Paths.ResourceServerBaseAddress);
            var client = new HttpClient(_webServerClient.CreateAuthorizingHandler(_accessToken));
            client.DefaultRequestHeaders.Accept
            .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            
            dynamic content = new JObject();
            dynamic consumer = new JObject();
            dynamic mobilePrefix = new JObject();
            dynamic address = new JObject();
            dynamic addressCountry = new JObject();
            
            consumer.name = "Savas";
            consumer.surname = "Manyasli";
            consumer.email = "savasmanyasli@mailinator.com";

            content.consumer = consumer;

            mobilePrefix.countryIso = "MLT";
            mobilePrefix.prefix = 356;

            consumer.mobilePrefix = mobilePrefix;
            consumer.mobileNumber = "99999957";

            addressCountry.isoAlpha3 = "MLT";
            address.country = addressCountry;

            content.consumerResidentialAddress = address;
   
            HttpContent myContent = new StringContent(content.ToString() , Encoding.UTF8,
                                    "application/json");
            HttpRequestMessage request = new HttpRequestMessage() { Content = myContent  };
            request.Method = HttpMethod.Post;
            request.RequestUri = new Uri(resourceServerUri, "/PagatudoAPI/Members/Signup" + "?lang=tr");
            try
            {
                var result = client.SendAsync(request).Result;
                result.EnsureSuccessStatusCode();

            }
            catch (Exception)
            {
                
                throw;
            }
         
        }



        private async static Task ChangePassword()
        {
            var resourceServerUri = new Uri(Paths.ResourceServerBaseAddress);
            var client = new HttpClient(_webServerClient.CreateAuthorizingHandler(_accessToken));
            client.DefaultRequestHeaders.Accept
            .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            dynamic content = new JObject();
            content.username = "savasmanyasli@mailinator.com";
    


            HttpContent myContent = new StringContent(content.ToString(), Encoding.UTF8,
                                    "application/json");
            HttpRequestMessage request = new HttpRequestMessage() { Content = myContent };
            request.Method = HttpMethod.Post;
            request.RequestUri = new Uri(resourceServerUri, "/PagatudoAPI/Members/Password" );
            try
            {
                var result = client.SendAsync(request).Result;
                result.EnsureSuccessStatusCode();

            }
            catch (Exception)
            {

                throw;
            }
        
        }


        private async static Task<JObject> GetUserProfile()
        {
            var client = new HttpClient(_webServerClient.CreateAuthorizingHandler(_accessToken));
            client.DefaultRequestHeaders.Accept
            .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            dynamic content  = new JObject();
            content.cid = "NRP";
            content.userName = "savasm@gmail.com";
            HttpContent myContent = new StringContent(content.ToString(), Encoding.UTF8,
                                    "application/json");
            try
            {
                var response = await client.GetAsync(new Uri(resourceServerUri, Paths.MePath ));
                var contents = await response.Content.ReadAsStringAsync();
                var retVal = JObject.Parse(contents);
                return retVal;
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }
    }
}
