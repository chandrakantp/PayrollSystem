using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace edueTree.Models
{   
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("edueTreeContainer")
        {
        }
    }
    public class CustomAuthorizationAttribute : AuthorizeAttribute
    {
        public string IdentityRoles
        {
            get { return _identityRoles ?? String.Empty; }
            set
            {
                _identityRoles = value;
                _identityRolesSplit = SplitString(value);
            }
        }

        private string _identityRoles;
        private string[] _identityRolesSplit = new string[0];

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var db = new dbContainer();
            //do the base class AuthorizeCore first
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }
            if (_identityRolesSplit.Length <= 0) return true;
            //get the UserManager
            using (new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
            {
                var id = HttpContext.Current.User.Identity.GetUserId();
                //get the Roles for this user
                var firstOrDefault = db.UserRoles.FirstOrDefault(q => q.userId == id);
                if (firstOrDefault == null) return false;
                var rroles = firstOrDefault.RoleId;
                // var roles = um.GetRoles(id);
                var aspNetRole = db.AspNetRoles.FirstOrDefault(a => a.Id == rroles);
                if (aspNetRole == null) return false;
                var asprole = aspNetRole.Name;

                if (_identityRolesSplit.Any(asprole.Contains))
                {
                    return true;
                }
            }
            return false;
        }
      
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {            
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
            else       
            {
                filterContext.Result = new RedirectResult("/Role/AccessDenied");
            }
        }

        protected static string[] SplitString(string original)
        {
            if (String.IsNullOrEmpty(original))
            {
                return new string[0];
            }
            var split = from piece in original.Split(',')
                        let trimmed = piece.Trim()
                        where !String.IsNullOrEmpty(trimmed)
                        select trimmed;
            return split.ToArray();
        }
    }
}