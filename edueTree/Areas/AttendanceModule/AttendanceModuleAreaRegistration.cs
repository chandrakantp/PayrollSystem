using System.Web.Mvc;

namespace edueTree.Areas.AttendanceModule
{
    public class AttendanceModuleAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AttendanceModule";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "AttendanceModule_default",
                "AttendanceModule/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}