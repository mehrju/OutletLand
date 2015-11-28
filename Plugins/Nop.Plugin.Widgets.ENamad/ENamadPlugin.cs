using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Nop.Core.Plugins;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;

namespace Nop.Plugin.Widgets.ENamad
{
    /// <summary>
    /// PLugin
    /// </summary>
    [AllowAnonymous]
    public class ENamadPlugin : BasePlugin, IWidgetPlugin
    {
        private readonly ISettingService _settingService;

        public ENamadPlugin(ISettingService settingService)
        {
            this._settingService = settingService;
        }

        /// <summary>
        /// Gets widget zones where this widget should be rendered
        /// </summary>
        /// <returns>Widget zones</returns>
        public IList<string> GetWidgetZones()
        {
            var setting = _settingService.LoadSetting<ENamadSettings>();
            return new List<string> { setting.PlacedIn };
        }

        /// <summary>
        /// Gets a route for provider configuration
        /// </summary>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "WidgetsENamad";
            routeValues = new RouteValueDictionary { { "Namespaces", "Nop.Plugin.Widgets.ENamad.Controllers" }, { "area", null } };
        }

        /// <summary>
        /// Gets a route for displaying widget
        /// </summary>
        /// <param name="widgetZone">Widget zone where it's displayed</param>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        public void GetDisplayWidgetRoute(string widgetZone, out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "PublicInfo";
            controllerName = "WidgetsENamad";
            routeValues = new RouteValueDictionary
            {
                {"Namespaces", "Nop.Plugin.Widgets.ENamad.Controllers"},
                {"area", null},
                {"widgetZone", widgetZone}
            };
        }
        
        /// <summary>
        /// Install plugin
        /// </summary>
        public override void Install()
        {
            //settings
            var settings = new ENamadSettings { PlacedIn = "left_side_column_after" };
            _settingService.SaveSetting(settings);
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.ENamad.PlacedIn", "Placed In");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.ENamad.IsActive", "Is Active");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.ENamad.ENamadCode", "ENamad Code");
            base.Install();
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override void Uninstall()
        {
            //settings
            _settingService.DeleteSetting<ENamadSettings>();

            //locales
            this.DeletePluginLocaleResource("Plugins.Widgets.ENamad.PlacedIn");
            this.DeletePluginLocaleResource("Plugins.Widgets.ENamad.IsActive");
            this.DeletePluginLocaleResource("Plugins.Widgets.ENamad.ENamadCode");
            base.Uninstall();
        }
    }
}
