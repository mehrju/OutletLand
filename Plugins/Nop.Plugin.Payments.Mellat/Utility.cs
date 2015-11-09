using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Payments.Mellat
{
    public static class Utility
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="codeType"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string ErrorCode(string codeType = "", string code = "")
        {
            if (codeType == "Installment")
            {
                switch (code)
                {
                    case "0":
                        return string.Format("تراکنش اعتباری با موفقیت انجام شد");
                    case "11":
                        return string.Format("شماره کارت نامعتبر است");
                    case "21":
                        return string.Format("پذیرنده نامعتبر است");
                    case "34":
                        return string.Format("خطای سیستمی");
                    case "421":
                        return string.Format("IPنامعتبر است");
                    case "418":
                        return string.Format("اشکال در تعریف اطلاعات مشتری");
                    case "14":
                        return string.Format("تعداد دفعات وارد کردن رمز بیش از حد مجاز است");
                    case "13":
                        return string.Format("رمز نامعتبر است");
                    case "35":
                        return string.Format("تاریخ نامعتبر است");
                    case "32":
                        return string.Format("فرمت اطلاعات وارد شده صحیح نمی باشد");
                    case "501":
                        return string.Format("فروشنده نامعتبر است");
                    case "41":
                        return string.Format("شماره درخواست تکراری است");
                    case "502":
                        return string.Format("شماره درخواست خرید صحیح نیست");
                    case "54":
                        return string.Format("تراکنش مرجع موجود نیست");
                    case "55":
                        return string.Format("تراکنش نامعتبر است");
                    default:
                        return string.Format("خطای درگاه پرداخت اقساطی به شماره : {0}", code);
                }
            }
            if (codeType == "bpPayRequest")
            {
                switch (code)
                {
                    case "17":
                        return string.Format("کاربر از انجام تراکنش منصرف شد");
                    case "11":
                    case "13":
                        return string.Format("اطلاعات وارد شده صحیح نمی باشد");
                    case "12":
                        return string.Format("موجودی کافی نیست");
                    case "15":
                        return string.Format("کارت نا معتبر است");
                    case "18":
                        return string.Format("تاریخ انقضای کارت گذشته است");
                    case "19":
                        return string.Format("مبلغ برداشت وجه بیش از حد مجاز است");
                    case "111":
                        return string.Format("صادر کننده کارت نامعتبر است");
                    case "113":
                        return string.Format("پاسخی از صادر کننده کارت دریافت نشد");
                    case "25":
                        return string.Format("مبلغ نا معتبر است");
                    case "33":
                        return string.Format("حساب نا معتبر است");
                    case "421":
                        return string.Format("IP نا معتبر است");
                    default:
                        return string.Format("خطای درگاه پرداخت اینترنتی بانک ملت به شماره : {0}", code);
                }
            }
            return "خطاهای متفرقه";
        }
    }
}
