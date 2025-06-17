using System.Web.Mvc;
using System.Web.Routing;

namespace edueTree
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Staffs", action = "StaffProfile", id = UrlParameter.Optional }
            );
        }

        //public static void RegisterArea(AreaRegistrationContext context)
        //{
        //   context.MapRoute(
        //        "Comment_default",
        //        "Comment/{controller}/{action}/{id}",
        //        new { controller = "Staffs", action = "StaffProfile", id = UrlParameter.Optional },
        //        new[] { "Demo.Web.Areas.Comment.Controllers" }
        //    );
        //}

    }
}
