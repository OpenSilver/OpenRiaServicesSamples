using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using OpenRiaServices.Server;
using OpenRiaServices.Server.Authentication;
using WpfCore.Web;

namespace SimpleApplication.Web
{
    public class User : OpenRiaServices.Server.Authentication.IUser
    {
        [Key]
        public string Name { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }

    [EnableClientAccess]
    public class AspNetIdentityAuthenticationDomainService : DomainService, IAuthentication<User>
    {
        public ApplicationSignInManager SignInManager => HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
        public ApplicationUserManager UserManager => HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
        private IAuthenticationManager AuthenticationManager => HttpContext.Current.GetOwinContext().Authentication;


        public AspNetIdentityAuthenticationDomainService()
        {

            

        }

        public User Login(string userName, string password, bool isPersistent, string customData)
        {
            // Create test user on first login
            // Never do this in production
            if (userName == "test")
            {
                var user = UserManager.FindByName(userName);
                if (user == null)
                {
                    // Create first user
                    user = new WpfCore.Web.Models.ApplicationUser { UserName = "test", Email = "none@dummy.com" };
                    var createResponse = UserManager.Create(user, "p@SSw0rd1234");
                    if (!createResponse.Succeeded)
                    {
                        throw new ValidationException(createResponse.Errors.First());
                    }
                }
            }

            var response = SignInManager.PasswordSignInAsync(userName, password, isPersistent, shouldLockout: false)
                .GetAwaiter()
                .GetResult();

            switch (response)
            {
                case Microsoft.AspNet.Identity.Owin.SignInStatus.Success:
                    return GetAuthenticatedUser(userName);
                case SignInStatus.RequiresVerification:
                    // customdata can be used to pass 2fa token
                    throw new NotImplementedException();
                 // 
                default:
                    return null;
            }
        }

        private User GetAnonymousUser()
        {
            return new User() { Name = "", Roles = Enumerable.Empty<string>() };
        }

        private User GetAuthenticatedUser(string userName)
        {
            // should find user from database and populate roles
            return new User() { Name = userName };
        }

        public User Logout()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return GetAnonymousUser();
        }

        public User GetUser()
        {
            var identity = HttpContext.Current.User.Identity;
            if (identity.IsAuthenticated)
            {
                return GetAuthenticatedUser(identity.Name);
            }
            else
            {
                return GetAnonymousUser();
            }
        }

        public void UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }

}