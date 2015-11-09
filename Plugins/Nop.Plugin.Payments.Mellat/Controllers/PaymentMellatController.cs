using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using Nop.Core;
using Nop.Plugin.Payments.Mellat.Models;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Logging;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;
using System.Net;

namespace Nop.Plugin.Payments.Mellat.Controllers
{
    public class PaymentMellatController : BasePaymentController
    {
        private readonly IWorkContext _workContext;
        private readonly IStoreService _storeService;
        private readonly ISettingService _settingService;
        private readonly IOrderService _orderService;
        private readonly IOrderProcessingService _orderProcessingService;
        private readonly ILogger _logger;
        private readonly IWebHelper _webHelper;
        private readonly IProductAttributeService _productAttributeService;
        private readonly MellatPaymentSettings _mellatPaymentSettings;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="installOrderId"></param>
        public void UpdateOrderId(int? orderId,int? installOrderId)
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
        /// 
        /// </summary>
        public PaymentMellatController()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workContext"></param>
        /// <param name="storeService"></param>
        /// <param name="settingService"></param>
        /// <param name="orderService"></param>
        /// <param name="orderProcessingService"></param>
        /// <param name="logger"></param>
        /// <param name="webHelper"></param>
        /// <param name="productAttributeService"></param>
        /// <param name="mellatPaymentSettings"></param>
        public PaymentMellatController(IWorkContext workContext,
            IStoreService storeService, 
            ISettingService settingService, 
            IOrderService orderService, 
            IOrderProcessingService orderProcessingService,
            ILogger logger, 
            IWebHelper webHelper,
            IProductAttributeService productAttributeService,
            MellatPaymentSettings mellatPaymentSettings
            )
        {
            this._workContext = workContext;
            this._storeService = storeService;
            this._settingService = settingService;
            this._orderService = orderService;
            this._orderProcessingService = orderProcessingService;
            this._logger = logger;
            this._webHelper = webHelper;
            this._productAttributeService = productAttributeService;
            this._mellatPaymentSettings = mellatPaymentSettings;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AdminAuthorize]
        [ChildActionOnly]
        // قابل استفاده در بخش مدیریتی سایت
        public ActionResult Configure()
        {
            //load settings for a chosen store scope
            var storeScope = GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var mellatPaymentSettings = _settingService.LoadSetting<MellatPaymentSettings>(storeScope);
            
            var model = new ConfigurationModel
            {
                TerminalId = mellatPaymentSettings.TerminalId,
                UserName = mellatPaymentSettings.UserName,
                UserPassword = mellatPaymentSettings.UserPassword,
                OrderId = mellatPaymentSettings.OrderId,
                AdditionalFee = mellatPaymentSettings.AdditionalFee,
                AdditionalFeePercentage = mellatPaymentSettings.AdditionalFeePercentage,
                ReturnFromMellatWithoutPaymentRedirectsToOrderDetailsPage =
                    mellatPaymentSettings.ReturnFromMellatWithoutPaymentRedirectsToOrderDetailsPage,
                BusinessEmail = mellatPaymentSettings.BusinessEmail,
                ActiveStoreScopeConfiguration = storeScope,
                InstallmentOrderId = mellatPaymentSettings.InstallmentOrderId
            };


            if (storeScope > 0)
            {
                model.TerminalId_OverrideForStore = _settingService.SettingExists(mellatPaymentSettings, x => x.TerminalId, storeScope);
                model.UserName_OverrideForStore = _settingService.SettingExists(mellatPaymentSettings, x => x.UserName, storeScope);
                model.UserPassword_OverrideForStore = _settingService.SettingExists(mellatPaymentSettings, x => x.UserPassword, storeScope);
                model.OrderId_OverrideForStore = _settingService.SettingExists(mellatPaymentSettings, x => x.OrderId, storeScope);
                model.AdditionalFee_OverrideForStore = _settingService.SettingExists(mellatPaymentSettings, x => x.AdditionalFee, storeScope);
                model.AdditionalFeePercentage_OverrideForStore = _settingService.SettingExists(mellatPaymentSettings, x => x.AdditionalFeePercentage, storeScope);
                model.ReturnFromMellatWithoutPaymentRedirectsToOrderDetailsPage_OverrideForStore = _settingService.SettingExists(mellatPaymentSettings, x => x.ReturnFromMellatWithoutPaymentRedirectsToOrderDetailsPage, storeScope);
                model.BusinessEmail_OverrideForStore = _settingService.SettingExists(mellatPaymentSettings, x => x.BusinessEmail, storeScope);

                model.InstallmentOrderId_OverrideForStore = _settingService.SettingExists(mellatPaymentSettings, x => x.InstallmentOrderId, storeScope);
            }

            return View("~/Plugins/Payments.Mellat/Views/PaymentMellat/Configure.cshtml", model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure(ConfigurationModel model)
        {
            if (!ModelState.IsValid)
                return Configure();

            //load settings for a chosen store scope
            var storeScope = GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var mellatPaymentSettings = _settingService.LoadSetting<MellatPaymentSettings>(storeScope);

            //save settings
            mellatPaymentSettings.TerminalId = model.TerminalId;
            mellatPaymentSettings.UserName = model.UserName;
            mellatPaymentSettings.UserPassword = model.UserPassword;
            mellatPaymentSettings.OrderId = model.OrderId;
            mellatPaymentSettings.BusinessEmail = model.BusinessEmail;
            mellatPaymentSettings.AdditionalFee = model.AdditionalFee;
            mellatPaymentSettings.AdditionalFeePercentage = model.AdditionalFeePercentage;
            mellatPaymentSettings.ReturnFromMellatWithoutPaymentRedirectsToOrderDetailsPage = model.ReturnFromMellatWithoutPaymentRedirectsToOrderDetailsPage;

            mellatPaymentSettings.InstallmentOrderId = model.InstallmentOrderId;


            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            if (model.TerminalId_OverrideForStore  || storeScope == 0)
                _settingService.SaveSetting(mellatPaymentSettings, x => x.TerminalId, storeScope, false); // بررسی شود
            else if (storeScope > 0)
                _settingService.DeleteSetting(mellatPaymentSettings, x => x.TerminalId, storeScope);
            if (model.UserName_OverrideForStore  || storeScope == 0)
                _settingService.SaveSetting(mellatPaymentSettings, x => x.UserName, storeScope, false); // بررسی شود
            else if (storeScope > 0)
                _settingService.DeleteSetting(mellatPaymentSettings, x => x.UserName, storeScope);

            if (model.UserPassword_OverrideForStore  || storeScope == 0)
                _settingService.SaveSetting(mellatPaymentSettings, x => x.UserPassword, storeScope, false); // بررسی شود
            else if (storeScope > 0)
                _settingService.DeleteSetting(mellatPaymentSettings, x => x.UserPassword, storeScope);

            if (model.OrderId_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(mellatPaymentSettings, x => x.OrderId, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(mellatPaymentSettings, x => x.OrderId, storeScope);

            if (model.BusinessEmail_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(mellatPaymentSettings, x => x.BusinessEmail, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(mellatPaymentSettings, x => x.BusinessEmail, storeScope);

            if (model.AdditionalFee_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(mellatPaymentSettings, x => x.AdditionalFee, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(mellatPaymentSettings, x => x.AdditionalFee, storeScope);

            if (model.AdditionalFeePercentage_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(mellatPaymentSettings, x => x.AdditionalFeePercentage, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(mellatPaymentSettings, x => x.AdditionalFeePercentage, storeScope);

            if (model.ReturnFromMellatWithoutPaymentRedirectsToOrderDetailsPage_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(mellatPaymentSettings, x => x.ReturnFromMellatWithoutPaymentRedirectsToOrderDetailsPage, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(mellatPaymentSettings, x => x.ReturnFromMellatWithoutPaymentRedirectsToOrderDetailsPage, storeScope);


            if (model.InstallmentOrderId_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(mellatPaymentSettings, x => x.InstallmentOrderId, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(mellatPaymentSettings, x => x.InstallmentOrderId, storeScope);


            //now clear settings cache
            _settingService.ClearCache();

            return Configure();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult PaymentInfo()
        {
            return View("~/Plugins/Payments.Mellat/Views/PaymentMellat/PaymentInfo.cshtml");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [NonAction]
        public override IList<string> ValidatePaymentForm(FormCollection form)
        {
            var warnings = new List<string>();
            return warnings; // بدلیل عدم دسترسی به مقادیر کارت مشتری
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [NonAction]
        public override ProcessPaymentRequest GetPaymentInfo(FormCollection form)
        {
            var paymentInfo = new ProcessPaymentRequest();
            return paymentInfo; // بدلیل عدم دسترسی به مقادیر کارت مشتری
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult CallBack()
        {
            string comment;
            var resCode = Request.Params["ResCode"];
            var orderId = int.Parse(Request.Params["OrderId"]??"0");
            var order = _orderService.GetOrderById(orderId);
            #region [After Call Mellat WebService]
            if (!string.IsNullOrEmpty(Request.Params["RefId"]) &&
                !string.IsNullOrEmpty(Request.Params["saleOrderId"]) &&
                !string.IsNullOrEmpty(Request.Params["SaleReferenceId"]))
            {
                var refId = Request.Params["RefId"];
                var saleOrderId = long.Parse(Request.Params["saleOrderId"]);
                var saleReferenceId = long.Parse(Request.Params["SaleReferenceId"]);
                if (resCode == "0")
                {
                    if (order != null)
                    {
                        try
                        {
                            #region [Mellat : bpVerifyRequest , bpSettleRequest]
                            var terminalId = _mellatPaymentSettings.TerminalId;
                            var userName = _mellatPaymentSettings.UserName;
                            var userPassword = _mellatPaymentSettings.UserPassword;
                            BypassCertificateError();
                            var bpService = new ir.shaparak.bpm.PaymentGatewayImplService();
                            var resultOfVerifyRequest = bpService.bpVerifyRequest(Int64.Parse(terminalId),
                                                        userName,
                                                        userPassword,
                                                        saleOrderId,
                                                        saleOrderId,
                                                        saleReferenceId);
                            var settleRequestArray = resultOfVerifyRequest.Split(',');
                            if (settleRequestArray[0] == "0")
                            {
                                var resultOfSettleRequest = bpService.bpSettleRequest(Int64.Parse(terminalId),
                                                            userName,
                                                            userPassword,
                                                            saleOrderId,
                                                            saleOrderId,
                                                            saleReferenceId);
                                if (resultOfSettleRequest != "0")
                                {
                                    throw new Exception();
                                }
                            }
                            else
                            {
                                throw new Exception();
                            }

                            #endregion
                            #region [mark order as paid]
                            if (_orderProcessingService.CanMarkOrderAsPaid(order))
                            {
                                //TODO : check saleOrderId is match by this Order.CaptureTransactionId
                                order.CaptureTransactionId = saleOrderId.ToString(CultureInfo.InvariantCulture);
                                order.SubscriptionTransactionId = string.Format("[RefId = {0}]", refId);

                                order.AuthorizationTransactionId = string.Format("[SaleReferenceId = {0}]",
                                    saleReferenceId.ToString(CultureInfo.InvariantCulture));
                                order.AuthorizationTransactionResult = string.Format("[ResCode = {0}]", resCode);

                                _orderService.UpdateOrder(order);
                                _orderProcessingService.MarkOrderAsPaid(order);
                            }
                            #endregion
                        }
                        catch
                        {
                            comment = string.Format("Error in Mellat Pay Verify or Pay Settle. OrderId= {0}", orderId);
                            _logger.Error(comment);
                            return RedirectToAction("Error", "PaymentMellat", new
                            {
                                result = comment,
                                cashRefId = Request.Params["SaleReferenceId"],
                                insRefId = "",
                                cashOrder = Request.Params["saleOrderId"],
                                insOrder = "",
                                id = order.Id
                            });
                        }
                        return RedirectToRoute("CheckoutCompleted", new { orderId = order.Id });
                    }
                    comment = string.Format("Order Not Found. OrderId= {0}", orderId);
                    _logger.Error(comment);
                    return RedirectToAction("Error", "PaymentMellat", new
                    {
                        result = comment,
                        cashRefId = Request.Params["SaleReferenceId"],
                        insRefId = "",
                        cashOrder = Request.Params["saleOrderId"],
                        insOrder = "",
                        id = orderId
                    });
                }
                comment = Utility.ErrorCode("bpPayRequest", resCode);
                _logger.Warning(comment);
                return RedirectToAction("Error", "PaymentMellat", new
                {
                    result = comment,
                    cashRefId = Request.Params["SaleReferenceId"],
                    insRefId = "",
                    cashOrder = Request.Params["saleOrderId"],
                    insOrder = "",
                    id = order.Id
                });
            }
            comment = Utility.ErrorCode("bpPayRequest", resCode);
            _logger.Error(comment);
            return RedirectToAction("Error", "PaymentMellat", new
            {
                result = comment,
                cashRefId = Request.Params["SaleReferenceId"],
                insRefId = "",
                cashOrder = Request.Params["saleOrderId"],
                insOrder = "",
                id = order.Id
            });
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public ActionResult Pay(string result)
        {
            ViewBag.result = result;
            return View("~/Plugins/Payments.Mellat/Views/PaymentMellat/Pay.cshtml");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public ActionResult PaymentResult(string result)
        {
            ViewBag.Message = result;
            return View("~/Plugins/Payments.Mellat/Views/PaymentMellat/PaymentResult.cshtml");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="insOrder"></param>
        /// <param name="id"></param>
        /// <param name="insRefId"></param>
        /// <param name="cashOrder"></param>
        /// <param name="cachRefId"></param>
        /// <returns></returns>
        public ActionResult Error(string result, string cachRefId, string insRefId, string cashOrder, string insOrder, int? id)
        {
            ViewBag.result = result;
            ViewBag.cachRefId = cachRefId;
            ViewBag.insRefId = insRefId;
            ViewBag.cashOrder = cashOrder;
            ViewBag.insOrder = insOrder;
            ViewBag.orderId = id??0;
            return View("~/Plugins/Payments.Mellat/Views/PaymentMellat/Error.cshtml");
        }

    }
}