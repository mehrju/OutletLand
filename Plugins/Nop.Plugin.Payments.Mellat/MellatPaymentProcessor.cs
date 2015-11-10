using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Core.Plugins;
using Nop.Plugin.Payments.Mellat.Controllers;
using Nop.Plugin.Payments.Mellat.InstallmentWS;
using Nop.Plugin.Payments.Mellat.Models;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Orders;
using Nop.Services.Payments;

namespace Nop.Plugin.Payments.Mellat
{
    /// <summary>
    /// 
    /// </summary>
    public class ConfigManager
    {
        private ConfigManager() { }
        private static ConfigManager _instance;
        public static ConfigManager Instance
        {
            get { return _instance ?? (_instance = new ConfigManager()); }
        }

        public long TerminalId { get { return 1; } }

        public string TerminalUserName { get { return "BazarcheMellat"; } }

        public string TerminalPassword { get { return "123456"; } }

        public static string PaymentGatewayUrl{ get { return "http://172.20.165.247:90/"; } }

        public static string GetPaymentGateway(string refId)
        {
            return PaymentGatewayUrl + "StartPay/Pay?refId=" + refId;
        }
    }
    /// <summary>
    /// Mellat payment processor
    /// </summary>
    public class MellatPaymentProcessor : BasePlugin, IPaymentMethod
    {
        #region Fields

        private readonly MellatPaymentSettings _mellatPaymentSettings;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        private readonly IOrderTotalCalculationService _orderTotalCalculationService;
        private readonly IOrderService _orderService;
        private readonly HttpContextBase _httpContext;
        private readonly IProductService _productService;
        private readonly IProductAttributeService _productAttributeService;

        #endregion

        #region Ctor

        public MellatPaymentProcessor(
            MellatPaymentSettings mellatPaymentSettings,
            ISettingService settingService,
            IWebHelper webHelper, 
            IOrderTotalCalculationService orderTotalCalculationService,
            IOrderService orderService,
            IProductService productService,
            IProductAttributeService productAttributeService,
            HttpContextBase httpContext)
        {
            _mellatPaymentSettings = mellatPaymentSettings;
            _settingService = settingService;
            _webHelper = webHelper;
            _orderTotalCalculationService = orderTotalCalculationService;
            _orderService = orderService;
            _productService = productService;
            _productAttributeService = productAttributeService;
            _httpContext = httpContext;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private string ToTwoDigit(int i)
        {
            var str = "";
            if (i < 10)
            {
                str = "0";
            }
            return (str + i);
        }
        /// <summary>
        /// 
        /// </summary>
        private string LocalDate
        {
            get
            {
                var str = new StringBuilder();

                str.Append(DateTime.Now.Year);
                str.Append(ToTwoDigit(DateTime.Now.Month));
                str.Append(ToTwoDigit(DateTime.Now.Day));

                return str.ToString();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private string LocalTime
        {
            get
            {
                var str = new StringBuilder();

                str.Append(ToTwoDigit(DateTime.Now.Hour));
                str.Append(ToTwoDigit(DateTime.Now.Minute));
                str.Append(ToTwoDigit(DateTime.Now.Second));

                return str.ToString();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        private void SendToError(int id)
        {
            var errorUrl = _webHelper.GetStoreLocation(false)
            + "Plugins/PaymentMellat/Error?result="
            + string.Format("خطا در پردازش سفارش، لطفا با پشتیبانی سامانه تماس بگیرید")
            + "&id="
            + id;
            _httpContext.Response.Redirect(errorUrl, true);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Process a payment
        /// </summary>
        /// <param name="processPaymentRequest">Payment info required for an order processing</param>
        /// <returns>Process payment result</returns>
        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest processPaymentRequest)
        {
            var result = new ProcessPaymentResult {NewPaymentStatus = PaymentStatus.Pending};
            return result;
        }

        /// <summary>
        /// Post process payment (used by payment gateways that require redirecting to a third-party URL)
        /// </summary>
        /// <param name="postProcessPaymentRequest">Payment info required for an order processing</param>
        /// public void
        public void PostProcessPayment(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            var order = postProcessPaymentRequest.Order;
            #region [Call bpMellat Web Service]

            var orderTotal = Math.Round(order.OrderTotal, 2);
            var mellatOrderId = _mellatPaymentSettings.OrderId;
            var terminalId = _mellatPaymentSettings.TerminalId;
            var userName = _mellatPaymentSettings.UserName;
            var userPassword = _mellatPaymentSettings.UserPassword;
            var price = Convert.ToInt64(orderTotal);
            const string description = "خرید از بازار اینترنتی";
            var callBackUrl = _webHelper.GetStoreLocation(false)
                              + "Plugins/PaymentMellat/CallBack?OrderId="
                              + order.Id;
            do
            {
                BypassCertificateError();
                var payDate = DateTime.Now.Year +
                              DateTime.Now.Month.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0') +
                              DateTime.Now.Day.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');
                var payTime = DateTime.Now.Hour.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0') +
                              DateTime.Now.Minute.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0') +
                              DateTime.Now.Second.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');


                var bpService = new ir.shaparak.bpm.PaymentGatewayImplService();
                var result = bpService.bpPayRequest(Int64.Parse(terminalId),
                    userName,
                    userPassword,
                    mellatOrderId,
                    price,
                    payDate,
                    payTime,
                    description,
                    callBackUrl,
                    0);

                string url, comment;
                var resultArray = result.Split(',');
                if (resultArray[0] == "0") // آماده برای انتقال به بانک
                {
                    order.CaptureTransactionId = mellatOrderId.ToString(CultureInfo.InvariantCulture);
                    _orderService.UpdateOrder(order);
                    UpdateOrderId(++mellatOrderId, null);
                    url = (_webHelper.GetStoreLocation(false)
                                             + "Plugins/PaymentMellat/Pay?result="
                                             + resultArray[1]);
                    _httpContext.Response.Redirect(url, true);
                }
                switch (result)
                {
                    case "41": // Order Id Already Exist
                        mellatOrderId++;
                        break;
                    case "421":// Invalid IP
                        comment = Utility.ErrorCode("bpPayRequest", result);
                        url = (_webHelper.GetStoreLocation(false)
                                                 + string.Format("Plugins/PaymentMellat/Error?result={0}&cashRefId={1}&insRefId={2}&cachOrder={3}&insOrder={4}&id={5}"
                                                     , comment, "", "", mellatOrderId, "", order.Id));
                        _httpContext.Response.Redirect(url, true);
                        break;
                    default:   // Other Errors
                        comment = Utility.ErrorCode("bpPayRequest");
                        url = (_webHelper.GetStoreLocation(false)
                                                 + string.Format("Plugins/PaymentMellat/Error?result={0}&cashRefId={1}&insRefId={2}&cachOrder={3}&insOrder={4}&id={5}"
                                                     , comment, "", "", mellatOrderId, "", order.Id));
                        _httpContext.Response.Redirect(url, true);
                        break;
                }
            }
            while (true);
            #endregion



        }

        public bool HidePaymentMethod(IList<ShoppingCartItem> cart)
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="installOrderId"></param>
        public void UpdateOrderId(int? orderId, int? installOrderId)
        {
            //settings
            var settings = new MellatPaymentSettings
            {
                OrderId = orderId??_mellatPaymentSettings.OrderId,
                TerminalId = _mellatPaymentSettings.TerminalId,
                UserName = _mellatPaymentSettings.UserName,
                UserPassword = _mellatPaymentSettings.UserPassword,
                AdditionalFee = _mellatPaymentSettings.AdditionalFee,
                BusinessEmail = _mellatPaymentSettings.BusinessEmail,
                AdditionalFeePercentage = _mellatPaymentSettings.AdditionalFeePercentage,
                ReturnFromMellatWithoutPaymentRedirectsToOrderDetailsPage = _mellatPaymentSettings.ReturnFromMellatWithoutPaymentRedirectsToOrderDetailsPage,

                InstallmentOrderId = installOrderId ?? _mellatPaymentSettings.InstallmentOrderId
            };
            _settingService.SaveSetting(settings);
        }

        /// <summary>
        /// 
        /// </summary>
        static void BypassCertificateError()
        {
            ServicePointManager.ServerCertificateValidationCallback +=
                (sender1, certificate, chain, sslPolicyErrors) => true;
        }

        /// <summary>
        /// Gets additional handling fee
        /// </summary>
        /// <param name="cart">Shoping cart</param>
        /// <returns>Additional handling fee</returns>
        /// مبلغ پول اضافی که به مبلغ کل افزوده می شود
        public decimal GetAdditionalHandlingFee(IList<ShoppingCartItem> cart)
        {
            var result = this.CalculateAdditionalFee(_orderTotalCalculationService, cart,
                _mellatPaymentSettings.AdditionalFee, _mellatPaymentSettings.AdditionalFeePercentage);
            return result;
        }

        /// <summary>
        /// Captures payment
        /// </summary>
        /// <param name="capturePaymentRequest">Capture payment request</param>
        /// <returns>Capture payment result</returns>
        public CapturePaymentResult Capture(CapturePaymentRequest capturePaymentRequest)
        {
            var result = new CapturePaymentResult();
            result.AddError("Capture method not supported");
            return result;
        }

        /// <summary>
        /// Refunds a payment
        /// </summary>
        /// <param name="refundPaymentRequest">Request</param>
        /// <returns>Result</returns>

        public RefundPaymentResult Refund(RefundPaymentRequest refundPaymentRequest)
        {
            var result = new RefundPaymentResult();
            result.AddError("Refund method not supported");
            return result;
        }

        /// <summary>
        /// Voids a payment
        /// </summary>
        /// <param name="voidPaymentRequest">Request</param>
        /// <returns>Result</returns>
        public VoidPaymentResult Void(VoidPaymentRequest voidPaymentRequest)
        {
            var result = new VoidPaymentResult();
            result.AddError("Void method not supported");
            return result;
        }

        /// <summary>
        /// Process recurring payment
        /// </summary>
        /// <param name="processPaymentRequest">Payment info required for an order processing</param>
        /// <returns>Process payment result</returns>
        public ProcessPaymentResult ProcessRecurringPayment(ProcessPaymentRequest processPaymentRequest)
        {
            var result = new ProcessPaymentResult();
            result.AddError("Recurring payment not supported");
            return result;
        }

        /// <summary>
        /// Cancels a recurring payment
        /// </summary>
        /// <param name="cancelPaymentRequest">Request</param>
        /// <returns>Result</returns>
        public CancelRecurringPaymentResult CancelRecurringPayment(CancelRecurringPaymentRequest cancelPaymentRequest)
        {
            var result = new CancelRecurringPaymentResult();
            result.AddError("Recurring payment not supported");
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether customers can complete a payment after order is placed but not completed (for redirection payment methods)
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>Result</returns>
        public bool CanRePostProcessPayment(Order order)
        {
            if (order == null)
                throw new ArgumentNullException("order");
            
            //let's ensure that at least 5 seconds passed after order is placed
            //P.S. there's no any particular reason for that. we just do it
            if ((DateTime.UtcNow - order.CreatedOnUtc).TotalSeconds < 5)
                return false;

            return true;
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
            controllerName = "PaymentMellat";
            routeValues = new RouteValueDictionary { { "Namespaces", "Nop.Plugin.Payments.Mellat.Controllers" }, { "area", null } };
        }

        /// <summary>
        /// Gets a route for payment info
        /// </summary>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        public void GetPaymentInfoRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "PaymentInfo";
            controllerName = "PaymentMellat";
            routeValues = new RouteValueDictionary { { "Namespaces", "Nop.Plugin.Payments.Mellat.Controllers" }, { "area", null } };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Type GetControllerType()
        {
            return typeof(PaymentMellatController);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Install()
        {
            //settings
            var settings = new MellatPaymentSettings
            {
                // تنظیمات پیش فرض
                TerminalId = "",
                UserName = "",
                UserPassword = "",
                OrderId = 1,

                InstallmentOrderId = 1
            };
            _settingService.SaveSetting(settings);

            //locales
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.Mellat.Fields.RedirectionTip", "برای تکمیل سفارش به درگاه پرداخت هدایت خواهید شد");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.Mellat.Fields.BusinessEmail", "Business Email");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.Mellat.Fields.BusinessEmail.Hint", "Specify your Mellat business email.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.Mellat.Fields.AdditionalFee", "هزینه های اضافی");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.Mellat.Fields.AdditionalFee.Hint", "هزینه های اضافی برای مطالبه از مشتریان خود وارد نمایید");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.Mellat.Fields.AdditionalFeePercentage", "هزینه اضافی. از درصد استفاده نمایید");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.Mellat.Fields.AdditionalFeePercentage.Hint", "تعیین اینکه آیا  درصد هزینه های اضافی به کل سفارش اعمال شود. اگر فعال نشود ، یک مقدار ثابت استفاده می شود");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.Mellat.Fields.ReturnFromMellatWithoutPaymentRedirectsToOrderDetailsPage", "بازگشت به صفحه جزئیات سفارش");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.Mellat.Fields.ReturnFromMellatWithoutPaymentRedirectsToOrderDetailsPage.Hint", "Enable if a customer should be redirected to the order details page when he clicks \"return to store\" link on Mellat site WITHOUT completing a payment");

            base.Install();
        }
        
        /// <summary>
        /// 
        /// </summary>
        public override void Uninstall()
        {
            //settings
            _settingService.DeleteSetting<MellatPaymentSettings>();

            //locales
            this.DeletePluginLocaleResource("Plugins.Payments.Mellat.Fields.RedirectionTip");
            this.DeletePluginLocaleResource("Plugins.Payments.Mellat.Fields.BusinessEmail");
            this.DeletePluginLocaleResource("Plugins.Payments.Mellat.Fields.BusinessEmail.Hint");
            this.DeletePluginLocaleResource("Plugins.Payments.Mellat.Fields.AdditionalFee");
            this.DeletePluginLocaleResource("Plugins.Payments.Mellat.Fields.AdditionalFee.Hint");
            this.DeletePluginLocaleResource("Plugins.Payments.Mellat.Fields.AdditionalFeePercentage");
            this.DeletePluginLocaleResource("Plugins.Payments.Mellat.Fields.AdditionalFeePercentage.Hint");
            this.DeletePluginLocaleResource("Plugins.Payments.Mellat.Fields.ReturnFromMellatWithoutPaymentRedirectsToOrderDetailsPage");
            this.DeletePluginLocaleResource("Plugins.Payments.Mellat.Fields.ReturnFromMellatWithoutPaymentRedirectsToOrderDetailsPage.Hint");
            
            base.Uninstall();
        }
        #endregion

        #region Properies

        /// <summary>
        /// Gets a value indicating whether capture is supported
        /// </summary>
        public bool SupportCapture
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether partial refund is supported
        /// </summary>
        public bool SupportPartiallyRefund
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether refund is supported
        /// </summary>
        public bool SupportRefund
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether void is supported
        /// </summary>
        public bool SupportVoid
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a recurring payment type of payment method
        /// </summary>
        public RecurringPaymentType RecurringPaymentType
        {
            get
            {
                return RecurringPaymentType.NotSupported;
            }
        }

        /// <summary>
        /// Gets a payment method type
        /// </summary>
        public PaymentMethodType PaymentMethodType
        {
            get
            {
                return PaymentMethodType.Redirection;
            }
        }

        /// <summary>
        /// Gets a value indicating whether we should display a payment information page for this plugin
        /// </summary>
        public bool SkipPaymentInfo
        {
            get
            {
                return false;
            }
        }

        #endregion
    }
}
