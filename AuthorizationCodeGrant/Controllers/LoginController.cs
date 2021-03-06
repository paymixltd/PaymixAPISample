﻿using AuthorizationCodeGrant.Service;
using PaymixSDK;
using DotNetOpenAuth.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AuthorizationCodeGrant.Controllers
{
    public class LoginController : Controller
    {

        
        // GET: Login
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Index")]
        public async Task<ActionResult> DoLogin()
        {
            var callbackURL =
                Url.Action("Process", "Login",
               routeValues: null ,
               protocol: Request.Url.Scheme );
            var userAuthorization = PaymixAuthService.GetClient().PrepareRequestUserAuthorization(scopes: new string[] { "USER_FINANCIAL",  "PUBLIC_DATA" }, returnTo: new Uri(callbackURL));
            userAuthorization.Send(HttpContext);
            Response.End();
            return View("~/Views/Home/Index.cshtml");
        }

        [HttpGet]
        [ActionName("Process")]
        public async Task<ActionResult> ProcessLogin()
        {
            var accessToken = Request.Form["AccessToken"];
            if (string.IsNullOrEmpty(accessToken))
            {
                    var authorizationState = PaymixAuthService.GetClient().ProcessUserAuthorization(Request);
                    if (authorizationState != null)
                    {
                        Session["veloxxa_access_token"] = authorizationState.AccessToken;
                    }
            }
            // This example keeps it in the session. You don't have to
            //Session["veloxxa_access_token"] = userAuthorization
            return RedirectToAction("Index","Home");
        }




    }
}