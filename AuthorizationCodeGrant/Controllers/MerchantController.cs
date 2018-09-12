using AuthorizationCodeGrant.Service;
using Newtonsoft.Json.Linq;
using PaymixSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AuthorizationCodeGrant.Controllers
{
    public class MerchantController : Controller
    {
        // GET: Merchant
        public ActionResult Index()
        {
            return View();
        }

        
        public  async Task<ActionResult> Send(string sendAmount)
        {
            if (Session["neropay_access_token"] != null)
            {
                var amount = Decimal.Parse(sendAmount).ToString("N2") ;
                
                // SAMPLE CALL THAT SENDS MONEY TO MERCHANT
                var result = await Service.PaymixResourceService.SendFundsToMerchant("MER_PIM", amount, "EUR",
                    Session["neropay_access_token"].ToString()
                    , new HttpClient(PaymixAuthService.GetClient().CreateAuthorizingHandler(Session["neropay_access_token"].ToString())));

                var profileData = ((JArray)JObject.Parse(result)["responseBody"])[0];
                ViewBag.Profile = profileData.ToObject<Profile>();
                // Load Dashboard
                /*;
                ViewBag.ApiResponse = body;
                */
            }

            return View();
        }
    }
}