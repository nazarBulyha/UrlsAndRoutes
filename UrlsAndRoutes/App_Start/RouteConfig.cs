using System.Web.Mvc;
using System.Web.Routing;
using UrlsAndRoutes.Infrastructure;

namespace UrlsAndRoutes
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.RouteExistingFiles = true;
            routes.MapMvcAttributeRoutes();

            routes.IgnoreRoute("Content/{filename}.html");

            routes.MapRoute("DiskFileRoute", "Content/StaticContent.html",
                new
                {
                    controller = "Customer",
                    action = "List"
                });

            routes.MapRoute("MyRoute", "{controller}/{action}",
                            namespaces: new[] { "UrlsAndRoutes.Controllers" });
            routes.MapRoute("MyOtherRoute", "App/{action}", new { controller = "Home" },
                namespaces: new[] { "UrlsAndRoutes.Controllers" });
        }
    }
}