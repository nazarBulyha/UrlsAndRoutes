using System.Web.Mvc;
using System.Web.Mvc.Routing.Constraints;
using System.Web.Routing;
using UrlsAndRoutes.Infractructure;

namespace UrlsAndRoutes
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "FirefoxRoute",
                url: "{*catchcall}",
                defaults: new
                {
                    controller = "Home",
                    action = "Index"
                },
                constraints: new
                {
                    custom = new UserAgentConstraint("Firefox")
                },
                namespaces: new[] {"UrlsAndRoutes.AdditionalControllers"}
            );

            routes.MapRoute(
                name: "MyRouteConfig",
                url: "{controller}/{action}/{id}/{*catchcall}",
                defaults: new
                {
                    controller = "Home",
                    action = "Index",
                    id = UrlParameter.Optional
                },
                namespaces: new[] { "UrlsAndRoutes.Controllers" },
                constraints: new
                {
                    controller = "^H.*",
                    action = "^Index$|^CustomVariable$",
                    httpMethod = new HttpMethodConstraint("GET","POST"),
                    id = new CompoundRouteConstraint(new IRouteConstraint[] {
                        new AlphaRouteConstraint(),
                        new MinLengthRouteConstraint(6)
                    })
                });
        }
    }
}
