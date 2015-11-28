using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Xml;
using Nop.Core;
using Nop.Plugin.Widgets.ENamad.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Widgets.ENamad.Controllers
{
    public class WidgetsENamadController : BasePluginController
    {
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IStoreService _storeService;
        private readonly ISettingService _settingService;
        private readonly ILocalizationService _localizationService;
        private readonly ILogger _logger;

        public WidgetsENamadController(IWorkContext workContext,
            IStoreContext storeContext,
            IStoreService storeService,
            ISettingService settingService,
            ILocalizationService localizationService,
            ILogger logger)
        {
            this._workContext = workContext;
            this._storeContext = storeContext;
            this._storeService = storeService;
            this._settingService = settingService;
            this._localizationService = localizationService;
            this._logger = logger;
        }
        #region Utility
        /// <summary>
        /// Gets widget zones where this widget should be rendered
        /// </summary>
        /// <returns>Widget zones</returns>
        private IList<SelectListItem> GetWidgetZones(string selected="")
        {
            var zones = new List<SelectListItem>();

            try
            {
                var doc = new XmlDocument();
                doc.Load(System.Web.HttpContext.Current.Server.MapPath("/Plugins/Widgets.ENamad/SupportedWidgetZones.xml"));

                var nodes = doc.SelectNodes("/SupportedWidgetZones/WidgetZone");
                zones.AddRange(from XmlNode node in nodes
                               select new SelectListItem
                               {
                                   Text = node.InnerText,
                                   Value = node.InnerText,
                                   Selected = (node.InnerText.ToLower() == selected)
                               });

                if (zones.Count == 0)
                {
                    _logger.Warning("No widget zones were loaded.");
                }
            }
            catch (FileNotFoundException fnf)
            {
                _logger.Error("Widget zone configuration file missing.", fnf);
            }
            catch (Exception ex)
            {
                _logger.Error("Error loading the widget zone configuration file.", ex);
            }

            return zones;
        }

        #endregion

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult Configure()
        {
            //load widgets from xml 
            ViewBag.Zones = GetWidgetZones();

            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var setting = _settingService.LoadSetting<ENamadSettings>(storeScope);
            var model = new ConfigurationModel
            {
                PlacedIn = setting.PlacedIn,
                IsActive = setting.IsActive,
                ENamadCode = setting.ENamadCode,
                ActiveStoreScopeConfiguration = storeScope
            };

            if (storeScope > 0)
            {
                model.PlacedIn_OverrideForStore = _settingService.SettingExists(setting, x => x.PlacedIn, storeScope);
                model.IsActive_OverrideForStore = _settingService.SettingExists(setting, x => x.IsActive, storeScope);
                model.ENamadCode_OverrideForStore = _settingService.SettingExists(setting, x => x.ENamadCode, storeScope);
            }
            
            return View("~/Plugins/Widgets.ENamad/Views/WidgetsENamad/Configure.cshtml", model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult Configure(ConfigurationModel model)
        {
            if (!ModelState.IsValid)
                return Configure();

            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var setting = _settingService.LoadSetting<ENamadSettings>(storeScope);

            setting.PlacedIn = model.PlacedIn;
            setting.IsActive = model.IsActive;
            setting.ENamadCode = model.ENamadCode;

            if (model.PlacedIn_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(setting, x => x.PlacedIn, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(setting, x => x.PlacedIn, storeScope);

            if (model.IsActive_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(setting, x => x.IsActive, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(setting, x => x.IsActive, storeScope);

            if (model.ENamadCode_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(setting, x => x.ENamadCode, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(setting, x => x.ENamadCode, storeScope);


            //now clear settings cache
            _settingService.ClearCache();

            SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));
            
            ViewBag.Zones = GetWidgetZones(model.PlacedIn);
            return Configure();
        }

        [ChildActionOnly]
        public ActionResult PublicInfo(string widgetZone, object additionalData = null)
        {
            var setting = _settingService.LoadSetting<ENamadSettings>(_storeContext.CurrentStore.Id);
            var model = new PublicInfoModel
            {
                IsActive = setting.IsActive,
                ENamadCode = setting.ENamadCode
            };
            return View("~/Plugins/Widgets.ENamad/Views/WidgetsENamad/PublicInfo.cshtml", model);
        }
    }
}