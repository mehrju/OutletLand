using System.Web.Mvc;
using System.Web.Routing;
using Nop.Web.Framework.Mvc.Routes;

namespace Nop.Plugin.Payments.mellat
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute("Plugin.Payments.Mellat.CallBack",
                 "Plugins/PaymentMellat/CallBack",
                 new { controller = "PaymentMellat", action = "CallBack" },
                 new[] { "Nop.Plugin.Payments.Mellat.Controllers" }

            );
            routes.MapRoute("Plugin.Payments.Mellat.Pay",
                 "Plugins/PaymentMellat/Pay",
                 new { controller = "PaymentMellat", action = "Pay", id = UrlParameter.Optional },
                 new[] { "Nop.Plugin.Payments.Mellat.Controllers" }

            );
            routes.MapRoute("Plugin.Payments.Mellat.Error",
                 "Plugins/PaymentMellat/Error",
                 new { controller = "PaymentMellat", action = "Error", id = UrlParameter.Optional },
                 new[] { "Nop.Plugin.Payments.Mellat.Controllers" }

            );
        }
        public int Priority
        {
            get
            {
                return 0;
            }
        }
    }
}
