using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Tms.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


          //  routes.MapRoute("Connector", "ELFinderConnector",
          // new { controller = "ELFinderConnector", action = "Main" });

          //  // Thumbnails
          //  routes.MapRoute("Thumbnauls", "Thumbnails/{target}",
          //      new { controller = "ELFinderConnector", action = "Thumbnails" });

          //  routes.MapRoute(
          //name: "EditPurchaseOrder",
          //url: "order/edit-purchase/{id}",
          //defaults: new { controller = "order", action = "EditPurchaseOrder", id = UrlParameter.Optional });

          //  routes.MapRoute(
          //name: "EditBoughtOrder",
          //url: "order/bought/view/{id}",
          //defaults: new { controller = "order", action = "EditBoughtOrder", id = UrlParameter.Optional });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }
}
