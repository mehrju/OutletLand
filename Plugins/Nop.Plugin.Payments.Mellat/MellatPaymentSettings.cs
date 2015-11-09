using Nop.Core.Configuration;

namespace Nop.Plugin.Payments.Mellat
{
    public class MellatPaymentSettings : ISettings
    {
        /// <summary>
        /// 
        /// </summary>
        public string TerminalId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string UserPassword { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string BusinessEmail { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether to "additional fee" is specified as percentage. true - percentage, false - fixed value.
        /// </summary>
        public bool AdditionalFeePercentage { get; set; }
        /// <summary>
        /// Additional fee
        /// </summary>
        public decimal AdditionalFee { get; set; }
        /// <summary>
        /// Enable if a customer should be redirected to the order details page
        /// when he clicks "return to store" link on Mellat site
        /// WITHOUT completing a payment
        /// </summary>
        public bool ReturnFromMellatWithoutPaymentRedirectsToOrderDetailsPage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int InstallmentOrderId { get; set; }
    }
}
