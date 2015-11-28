using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Widgets.ENamad.Models
{
    public class PublicInfoModel : BaseNopModel
    {
        public bool IsActive { get; set; }
        public string ENamadCode { get; set; }
    }
}