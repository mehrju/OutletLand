using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.ComponentModel; 

namespace Nop.Plugin.Payments.Mellat.Models
{
    public class ConfigurationModel : BaseNopModel
    {
        [DisplayName("شماره ترمینال")]
        public string TerminalId { get; set; }
        public bool TerminalId_OverrideForStore { get; set; }
        [DisplayName("نام کاربری")]
        public string UserName { get; set; }
        public bool UserName_OverrideForStore { get; set; }
        [DisplayName("شماره درخواست بانک ملت")]
        public int OrderId { get; set; } // منحصر به فرد در هر درخواست
        public bool OrderId_OverrideForStore { get; set; }
        [NopResourceDisplayName("رمز عبور")]
        public string UserPassword { get; set; }
        public bool UserPassword_OverrideForStore { get; set; }
        [NopResourceDisplayName("Plugins.Payments.Mellat.Fields.BusinessEmail")]
        public string BusinessEmail { get; set; }
        public bool BusinessEmail_OverrideForStore { get; set; }
        public int ActiveStoreScopeConfiguration { get; set; }
        [NopResourceDisplayName("Plugins.Payments.Mellat.Fields.AdditionalFee")]
        public decimal AdditionalFee { get; set; }
        public bool AdditionalFee_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.Mellat.Fields.AdditionalFeePercentage")]
        public bool AdditionalFeePercentage { get; set; }
        public bool AdditionalFeePercentage_OverrideForStore { get; set; }
        [NopResourceDisplayName("Plugins.Payments.Mellat.Fields.ReturnFromMellatWithoutPaymentRedirectsToOrderDetailsPage")]
        public bool ReturnFromMellatWithoutPaymentRedirectsToOrderDetailsPage { get; set; }
        public bool ReturnFromMellatWithoutPaymentRedirectsToOrderDetailsPage_OverrideForStore { get; set; }
        [DisplayName("شماره درخواست سامانه اقساطی")]
        public int InstallmentOrderId { get; set; }
        public bool InstallmentOrderId_OverrideForStore { get; set; }
    }
}