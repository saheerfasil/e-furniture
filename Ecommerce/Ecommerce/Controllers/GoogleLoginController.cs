using DotNetOpenAuth.GoogleOAuth2;
using Microsoft.AspNet.Membership.OpenAuth;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce.Controllers
{
    public class GoogleLoginController : Controller
    {

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OpenAuth.RequestAuthentication(Provider, ReturnUrl);
            }
        }
        // GET: GoogleLogin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RedirectToGoogle()
        {
            string provider = "google";
            string returnUrl = "";
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }
        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            string ProviderName = OpenAuth.GetProviderNameFromCurrentRequest();

            if (ProviderName == null || ProviderName == "")
            {
                NameValueCollection nvs = Request.QueryString;
                if (nvs.Count > 0)
                {
                    if (nvs["state"] != null)
                    {
                        NameValueCollection provideritem = HttpUtility.ParseQueryString(nvs["state"]);
                        if (provideritem["__provider__"] != null)
                        {
                            ProviderName = provideritem["__provider__"];
                        }
                    }
                }
            }

            GoogleOAuth2Client.RewriteRequest();

            var redirectUrl = Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl });
            var retUrl = returnUrl;
            var authResult = OpenAuth.VerifyAuthentication(redirectUrl);

            if (!authResult.IsSuccessful)
            {
                return Redirect(Url.Action("Account", "Login"));
            }

            
            if (OpenAuth.Login(authResult.Provider, authResult.ProviderUserId, createPersistentCookie: false))
            {
                return Redirect(Url.Action("Index", "Home"));
            }

            
            string ProviderUserId = authResult.ProviderUserId;
            string ProviderUserName = authResult.UserName;

            string Email = null;
            if (Email == null && authResult.ExtraData.ContainsKey("email"))
            {
                Email = authResult.ExtraData["email"];
            }

            if (User.Identity.IsAuthenticated)
            {
                
                OpenAuth.AddAccountToExistingUser(ProviderName, ProviderUserId, ProviderUserName, User.Identity.Name);
                return Redirect(Url.Action("Index", "Home"));
            }
            else
            {
               
                string membershipUserName = Email ?? ProviderUserId;
                var createResult = OpenAuth.CreateUser(ProviderName, ProviderUserId, ProviderUserName, membershipUserName);

                if (!createResult.IsSuccessful)
                {
                    ViewBag.Message = "User cannot be created";
                    return View();
                }
                else
                {
                    
                    if (OpenAuth.Login(ProviderName, ProviderUserId, createPersistentCookie: false))
                    {
                        return Redirect(Url.Action("Index", "Home"));
                    }
                }
            }
            return View();
        }

    }
}