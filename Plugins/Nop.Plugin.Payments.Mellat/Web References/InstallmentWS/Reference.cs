﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.18408.
// 
#pragma warning disable 1591

namespace Nop.Plugin.Payments.Mellat.InstallmentWS {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="InstallmentWSSoap", Namespace="http://tempuri.org/")]
    public partial class InstallmentWS : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback HelloWorldOperationCompleted;
        
        private System.Threading.SendOrPostCallback PayRequestOperationCompleted;
        
        private System.Threading.SendOrPostCallback VerifyRequestOperationCompleted;
        
        private System.Threading.SendOrPostCallback PayGroupRequestOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public InstallmentWS() {
            this.Url = global::Nop.Plugin.Payments.Mellat.Properties.Settings.Default.Nop_Plugin_Payments_Mellat_InstallmentWS_InstallmentWS;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event HelloWorldCompletedEventHandler HelloWorldCompleted;
        
        /// <remarks/>
        public event PayRequestCompletedEventHandler PayRequestCompleted;
        
        /// <remarks/>
        public event VerifyRequestCompletedEventHandler VerifyRequestCompleted;
        
        /// <remarks/>
        public event PayGroupRequestCompletedEventHandler PayGroupRequestCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/HelloWorld", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string HelloWorld() {
            object[] results = this.Invoke("HelloWorld", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void HelloWorldAsync() {
            this.HelloWorldAsync(null);
        }
        
        /// <remarks/>
        public void HelloWorldAsync(object userState) {
            if ((this.HelloWorldOperationCompleted == null)) {
                this.HelloWorldOperationCompleted = new System.Threading.SendOrPostCallback(this.OnHelloWorldOperationCompleted);
            }
            this.InvokeAsync("HelloWorld", new object[0], this.HelloWorldOperationCompleted, userState);
        }
        
        private void OnHelloWorldOperationCompleted(object arg) {
            if ((this.HelloWorldCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.HelloWorldCompleted(this, new HelloWorldCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/PayRequest", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string PayRequest(long terminalId, string userName, string userPassword, long orderId, long amount, long installmentNumber, InstallmentIntervalType installmentInterval, string localDate, string localTime, string additionalData, string callBackUrl, string sellerId) {
            object[] results = this.Invoke("PayRequest", new object[] {
                        terminalId,
                        userName,
                        userPassword,
                        orderId,
                        amount,
                        installmentNumber,
                        installmentInterval,
                        localDate,
                        localTime,
                        additionalData,
                        callBackUrl,
                        sellerId});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void PayRequestAsync(long terminalId, string userName, string userPassword, long orderId, long amount, long installmentNumber, InstallmentIntervalType installmentInterval, string localDate, string localTime, string additionalData, string callBackUrl, string sellerId) {
            this.PayRequestAsync(terminalId, userName, userPassword, orderId, amount, installmentNumber, installmentInterval, localDate, localTime, additionalData, callBackUrl, sellerId, null);
        }
        
        /// <remarks/>
        public void PayRequestAsync(long terminalId, string userName, string userPassword, long orderId, long amount, long installmentNumber, InstallmentIntervalType installmentInterval, string localDate, string localTime, string additionalData, string callBackUrl, string sellerId, object userState) {
            if ((this.PayRequestOperationCompleted == null)) {
                this.PayRequestOperationCompleted = new System.Threading.SendOrPostCallback(this.OnPayRequestOperationCompleted);
            }
            this.InvokeAsync("PayRequest", new object[] {
                        terminalId,
                        userName,
                        userPassword,
                        orderId,
                        amount,
                        installmentNumber,
                        installmentInterval,
                        localDate,
                        localTime,
                        additionalData,
                        callBackUrl,
                        sellerId}, this.PayRequestOperationCompleted, userState);
        }
        
        private void OnPayRequestOperationCompleted(object arg) {
            if ((this.PayRequestCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.PayRequestCompleted(this, new PayRequestCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/VerifyRequest", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string VerifyRequest(long terminalId, string userName, string userPassword, long orderId, long saleOrderId, string saleReferenceCode) {
            object[] results = this.Invoke("VerifyRequest", new object[] {
                        terminalId,
                        userName,
                        userPassword,
                        orderId,
                        saleOrderId,
                        saleReferenceCode});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void VerifyRequestAsync(long terminalId, string userName, string userPassword, long orderId, long saleOrderId, string saleReferenceCode) {
            this.VerifyRequestAsync(terminalId, userName, userPassword, orderId, saleOrderId, saleReferenceCode, null);
        }
        
        /// <remarks/>
        public void VerifyRequestAsync(long terminalId, string userName, string userPassword, long orderId, long saleOrderId, string saleReferenceCode, object userState) {
            if ((this.VerifyRequestOperationCompleted == null)) {
                this.VerifyRequestOperationCompleted = new System.Threading.SendOrPostCallback(this.OnVerifyRequestOperationCompleted);
            }
            this.InvokeAsync("VerifyRequest", new object[] {
                        terminalId,
                        userName,
                        userPassword,
                        orderId,
                        saleOrderId,
                        saleReferenceCode}, this.VerifyRequestOperationCompleted, userState);
        }
        
        private void OnVerifyRequestOperationCompleted(object arg) {
            if ((this.VerifyRequestCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.VerifyRequestCompleted(this, new VerifyRequestCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/PayGroupRequest", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string PayGroupRequest(long terminalId, string userName, string userPassword, long orderId, string xmlOrderList, string localDate, string localTime, string additionalData, string callBackUrl) {
            object[] results = this.Invoke("PayGroupRequest", new object[] {
                        terminalId,
                        userName,
                        userPassword,
                        orderId,
                        xmlOrderList,
                        localDate,
                        localTime,
                        additionalData,
                        callBackUrl});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void PayGroupRequestAsync(long terminalId, string userName, string userPassword, long orderId, string xmlOrderList, string localDate, string localTime, string additionalData, string callBackUrl) {
            this.PayGroupRequestAsync(terminalId, userName, userPassword, orderId, xmlOrderList, localDate, localTime, additionalData, callBackUrl, null);
        }
        
        /// <remarks/>
        public void PayGroupRequestAsync(long terminalId, string userName, string userPassword, long orderId, string xmlOrderList, string localDate, string localTime, string additionalData, string callBackUrl, object userState) {
            if ((this.PayGroupRequestOperationCompleted == null)) {
                this.PayGroupRequestOperationCompleted = new System.Threading.SendOrPostCallback(this.OnPayGroupRequestOperationCompleted);
            }
            this.InvokeAsync("PayGroupRequest", new object[] {
                        terminalId,
                        userName,
                        userPassword,
                        orderId,
                        xmlOrderList,
                        localDate,
                        localTime,
                        additionalData,
                        callBackUrl}, this.PayGroupRequestOperationCompleted, userState);
        }
        
        private void OnPayGroupRequestOperationCompleted(object arg) {
            if ((this.PayGroupRequestCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.PayGroupRequestCompleted(this, new PayGroupRequestCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18408")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public enum InstallmentIntervalType {
        
        /// <remarks/>
        Weekly,
        
        /// <remarks/>
        Monthly,
        
        /// <remarks/>
        Quarterly,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    public delegate void HelloWorldCompletedEventHandler(object sender, HelloWorldCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class HelloWorldCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal HelloWorldCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    public delegate void PayRequestCompletedEventHandler(object sender, PayRequestCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class PayRequestCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal PayRequestCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    public delegate void VerifyRequestCompletedEventHandler(object sender, VerifyRequestCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class VerifyRequestCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal VerifyRequestCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    public delegate void PayGroupRequestCompletedEventHandler(object sender, PayGroupRequestCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class PayGroupRequestCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal PayGroupRequestCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591