using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OwinOauthAspNetWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace OwinOauthAspNetWebApi.Controllers
{

    public class AccountController : ApiController
    {
        [Route("api/Account/Register")]
        [HttpPost]
        public async Task<IdentityResult> Register(AccountModel model)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            IdentityResult result;

            var userstore = new UserStore<ApplicationUser>(context);
            var UserManager = new UserManager<ApplicationUser>(userstore);
            var user = new ApplicationUser() { UserName = model.UserName, Email = model.Email };
            UserManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 3
            };

            result = await UserManager.CreateAsync(user, model.Password);

            return result;

        }

        [HttpGet]
        [Authorize]
        [Route("api/GetUserClaim")]
        public async Task<AccountModel> GetUserClaim()
        {
            var identityClaims = (ClaimsIdentity)User.Identity;
            AccountModel model = new AccountModel()
            {
                Email = identityClaims.FindFirst("Email").Value,
                UserName = identityClaims.FindFirst("UserName").Value,
                LoggedOn = identityClaims.FindFirst("LoggedOn").Value
            };

            return model;
        }
    }
}
