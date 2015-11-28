using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Widgets.ENamad.Models
{
    public class ConfigurationModel : BaseNopModel
    {
        public int ActiveStoreScopeConfiguration { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.ENamad.PlacedIn")]
        [UIHint(" ")]
        [Required]
        public string PlacedIn { get; set; }
        public bool PlacedIn_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.ENamad.IsActive")]
        [UIHint(" ")]
        public bool IsActive { get; set; }
        public bool IsActive_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.ENamad.ENamadCode")]
        [AllowHtml]
        [UIHint(" ")]
        public string ENamadCode { get; set; }
        public bool ENamadCode_OverrideForStore { get; set; }


    }
}