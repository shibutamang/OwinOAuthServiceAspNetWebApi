using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.OAuth;
using OwinOauthAspNetWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace OwinOauthAspNetWebApi
{
    public class ApplicationOAuthProvider: OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(userStore);
            var user = await userManager.FindAsync(context.UserName, context.Password);

            if (user != null)
            {
                var userClaims = new ClaimsIdentity(context.Options.AuthenticationType);
                userClaims.AddClaim(new Claim("UserName", user.UserName));
                userClaims.AddClaim(new Claim("Email", user.Email));
                userClaims.AddClaim(new Claim("LoggedOn", DateTime.Now.ToString()));
                context.Validated(userClaims);
            }
            else
            {
                return;
            }
        }
    }
}