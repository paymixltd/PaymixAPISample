using AuthorizationCodeGrant.Service;
using PaymixSDK;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AuthorizationCodeGrant.Controllers
{
    public class HomeController : Controller
    {
        

        public async Task<ActionResult> Index()
        {

            if(Session["veloxxa_access_token"]!=null)
            {
                
                // SAMPLE CALL THAT GETS OPEN EWALLETS
               var client = new HttpClient(PaymixAuthService.GetClient().CreateAuthorizingHandler(Session["veloxxa_access_token"].ToString()));



                try
                {
                    var result = await client.GetStringAsync(new Uri(PaymixAuthService.URL + "Members/Account"));
                    JObject jsonAccount = JObject.Parse(result);
                    var accountlist = (JArray)(jsonAccount["responseBody"]);
                    ViewBag.AccountList = accountlist.ToObject<List<Account>>();

                }
                catch (Exception ex)
                {
                    
                    
                }
               

                // SAMPLE CALL THAT GETS CUSTOMER KYC DETAILS
              /*  result = await client.GetStringAsync(new Uri(PaymixAuthService.URL + "/Members/Profile"));
                var profileData = ((JArray)JObject.Parse(result)["responseBody"])[0];
                ViewBag.Profile = profileData.ToObject<Profile>();*/
                // Load Dashboard

                /*;
                  
                  ViewBag.ApiResponse = body;
                  */
            }

            return View();
          
        }



    }
}