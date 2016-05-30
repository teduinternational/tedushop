using System.Web.Mvc;
using System.Web.Routing;

namespace TeduShop.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
      name: "Search",
      url: "tim-kiem.html",
      defaults: new { controller = "Product", action = "Search", id = UrlParameter.Optional },
      namespaces: new string[] { "TeduShop.Web.Controllers" }
  );
            routes.MapRoute(
         name: "Login",
         url: "dang-nhap.html",
         defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional },
         namespaces: new string[] { "TeduShop.Web.Controllers" }
     );
            routes.MapRoute(
           name: "About",
           url: "gioi-thieu.html",
           defaults: new { controller = "About", action = "Index", id = UrlParameter.Optional },
           namespaces: new string[] { "TeduShop.Web.Controllers" }
       );
            routes.MapRoute(
             name: "Product Category",
             url: "{alias}.pc-{id}.html",
             defaults: new { controller = "Product", action = "Category", id = UrlParameter.Optional },
               namespaces: new string[] { "TeduShop.Web.Controllers" }
         );

            routes.MapRoute(
             name: "Product",
             url: "{alias}.p-{productId}.html",
             defaults: new { controller = "Product", action = "Detail", productId = UrlParameter.Optional },
               namespaces: new string[] { "TeduShop.Web.Controllers" }
         );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                  namespaces: new string[] { "TeduShop.Web.Controllers" }
            );
        }
    }
}