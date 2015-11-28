using Nop.Core.Configuration;

namespace Nop.Plugin.Widgets.ENamad
{
    public class ENamadSettings : ISettings
    {
        public string PlacedIn { get; set; }
        public bool IsActive { get; set; }
        public string ENamadCode { get; set; }
    }
}