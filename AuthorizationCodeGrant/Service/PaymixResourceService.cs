﻿using PaymixSDK;
using DotNetOpenAuth.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Text;

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


        public static async Task<string> SendFundsToMerchant(string merchantCode, string Amount , string Currency , string token, HttpClient authorizedClient )
        {


            authorizedClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            dynamic content = new JObject();
            
            content.merchantCode = merchantCode;
            content.amount = Amount;
            content.currency = Currency;
            content.description = "Transaction Narrative from Merchant";
            content.merchantTransactionId = Guid.NewGuid().ToString("D"); // Make sure this id is  unique to avoid merchant validation exception. This check enforces the customer's duplicate spending.

        
            HttpContent myContent = new StringContent(content.ToString(), Encoding.UTF8,
                                    "application/json");
            try
            {
                var response = await authorizedClient.PostAsync(new Uri(PaymixSDK.Paths.ResourceServerBaseAddress + "/v2/Members/Transfer/Merchant"), myContent);
                var contents = await response.Content.ReadAsStringAsync();

                dynamic jsonToken = JObject.Parse(contents);
                var returnToken = (JArray)(jsonToken["responseBody"]);
                var confirmRespone = ConfirmPayment(returnToken[0].Value<string>(), authorizedClient);
                
                return contents;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public static async Task<string> ConfirmPayment(string token, HttpClient authorizedClient)
        {


            authorizedClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            dynamic content = new JObject();

            content.token = token;
            content.otp = 262309;

            HttpContent myContent = new StringContent(content.ToString(), Encoding.UTF8,
                                    "application/json");
            try
            {
                var response = await authorizedClient.PostAsync(new Uri(PaymixSDK.Paths.ResourceServerBaseAddress + "/v2/Members/Transfer/Merchant/Confirm"), myContent);
                var contents = await response.Content.ReadAsStringAsync();

                return contents;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

    }
}