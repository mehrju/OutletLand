using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace IUtility
{
    public static class CommonMethods
    {
        #region String Method Extensions (8)

        /// <summary>
        /// Truncate and Remove Html Tags. 
        /// [Mabna Method]
        /// </summary>
        /// <param name="s">string to begin with</param>
        /// <param name="lenght">Number of characters to return</param>
        /// <returns>int</returns>
        public static string Truncate(this string s, int lenght)
        {
            s = Regex.Replace(s, @"<(.|\n)*?>", string.Empty);
            s = s.Replace("\\", "");
            s = s.Replace("\"", "");
            if (s.Length > lenght)
            {
                s = s.Substring(0, lenght) + " ...";
            }
            return s;
        }

        /// <summary>
        /// Count all words in a given string. 
        /// [Mabna Method]
        /// </summary>
        /// <param name="input">string to begin with</param>
        /// <returns>int</returns>
        public static int WordCount(this string input)
        {
            int count;
            try
            {
                // Exclude whitespaces, Tabs and line breaks
                var re = new Regex(@"[^\s]+");
                var matches = re.Matches(input);
                count = matches.Count;
            }
            catch (Exception)
            {
                count = -1;
            }
            return count;
        }

        /// <summary>
        /// Returns the last few characters of the string with a length
        /// specified by the given parameter. If the string's length is less than the 
        /// given length the complete string is returned. If length is zero or 
        /// less an empty string is returned.
        /// [Mabna Method]
        /// </summary>
        /// <param name="s">the string to process</param>
        /// <param name="length">Number of characters to return</param>
        /// <returns></returns>
        public static string Right(this string s, int length)
        {
            length = Math.Max(length, 0);
            return s.Length > length ? s.Substring(s.Length - length, length) : s;
        }

        /// <summary>
        /// Returns the first few characters of the string with a length
        /// specified by the given parameter. If the string's length is less than the 
        /// given length the complete string is returned. If length is zero or 
        /// less an empty string is returned
        /// </summary>
        /// <param name="s">the string to process</param>
        /// <param name="length">Number of characters to return</param>
        /// <returns></returns>
        public static string Left(this string s, int length)
        {
            length = Math.Max(length, 0);
            return s.Length > length ? s.Substring(0, length) : s;
        }

        /// <summary>
        /// check input string is valid url. 
        /// [Mabna Method]
        /// </summary>
        /// <param name="s">string to validate</param>
        /// <returns>bool</returns>
        public static bool IsValidUrl(this string s)
        {
            var rx = new Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
            return rx.IsMatch(s);
        }

        /// <summary>
        /// check input string is valid email. 
        /// [Mabna Method]
        /// </summary>
        /// <param name="s">string to validate</param>
        /// <returns>bool</returns>
        public static bool IsValidEmailAddress(this string s)
        {
            var regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return regex.IsMatch(s);
        }

        /// <summary>
        /// Send an email using the supplied string.
        /// [Mabna Method]
        /// </summary>
        /// <param name="body">String that will be used i the body of the email.</param>
        /// <param name="subject">Subject of the email.</param>
        /// <param name="sender">The email address from which the message was sent.</param>
        /// <param name="toAddress"></param>
        /// <param name="isHtml"></param>
        /// <returns>A boolean value indicating the success of the email send.</returns>
        public static bool Email(this string body, string subject, string sender, string toAddress, bool isHtml)
        {
            try
            {
                // To
                var mailMsg = new MailMessage();
                string[] to = toAddress.Split(',');
                foreach (string s in to)
                {
                    if (s.IsValidEmailAddress())
                    {
                        mailMsg.To.Add(s);
                    }
                }

                // From
                var mailAddress = new MailAddress(sender);
                mailMsg.From = mailAddress;

                // Subject and Body
                mailMsg.Subject = subject;
                mailMsg.Body = body;
                mailMsg.IsBodyHtml = isHtml;

                // Init SmtpClient and send
                var smtpClient = new SmtpClient();

                smtpClient.Send(mailMsg);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Send an email using the supplied string.
        /// [Mabna Method]
        /// </summary>
        /// <param name="body">String that will be used i the body of the email.</param>
        /// <param name="subject">Subject of the email.</param>
        /// <param name="sender">The email address from which the message was sent.</param>
        /// <param name="toAddress"></param>
        /// <param name="isHtml"></param>
        /// <param name="attachment"></param>
        /// <returns>A boolean value indicating the success of the email send.</returns>
        public static bool Email(this string body, string subject, string sender, string toAddress, bool isHtml, string attachment)
        {
            try
            {
                // To
                var mailMsg = new MailMessage();
                string[] to = toAddress.Split(',');
                foreach (string s in to)
                {
                    if (s.IsValidEmailAddress())
                    {
                        mailMsg.To.Add(s);
                    }
                }

                // From
                var mailAddress = new MailAddress(sender);
                mailMsg.From = mailAddress;

                // Subject and Body
                mailMsg.Subject = subject;
                mailMsg.Body = body;
                mailMsg.IsBodyHtml = isHtml;

                //attachment 
                var a = new Attachment(attachment);
                mailMsg.Attachments.Add(a);

                // Init SmtpClient and send
                var smtpClient = new SmtpClient();

                smtpClient.Send(mailMsg);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public static int ToInt32(this object o)
        {
            try
            {
                return Convert.ToInt32(o);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// Remove BR Tags from input string. 
        /// [Mabna Method]
        /// </summary>
        /// <param name="s">string to validate</param>
        /// <returns>string</returns>
        public static string CleanBrTags(this string s)
        {
            return s.Replace(@"<br \>", "");
        }

        /// <summary>
        /// Screen Scrape Html from url.
        /// [Mabna Method]
        /// </summary>
        /// <param name="url">Source file Internet Url</param>
        /// <returns>string</returns>
        public static string ScreenScrapeHtml(this string url)
        {
            var objRequest = WebRequest.Create(url);
            var sr = new StreamReader(objRequest.GetResponse().GetResponseStream());
            string result = sr.ReadToEnd();
            sr.Close();
            return result.ToLower();
        }


        public static string ToPersianNumbers(this string s)
        {
            return s.Replace("0", "۰").Replace("1", "۱").Replace("2", "۲").Replace("3", "۳").Replace("4", "۴").Replace("5", "۵").Replace("6", "۶").Replace("7", "۷").Replace("8", "۸").Replace("9", "۹");
        }

        public static string ToEnglishNumbers(this string s)
        {
            return s.Replace("۰","0").Replace("۱","1").Replace("۲","2").Replace("۳","3").Replace("۴","4").Replace("۵","5").Replace("۶","6").Replace("۷","7").Replace("۸","8").Replace("۹","9");
        }

        public static string ToPersianContent(this string s)
        {
            return (s.Replace("ي", "ی").Replace("ك", "ک").Replace("ة", "ه"));
        }

        #endregion

        public static void Insert(this HtmlHead h, string script, bool addScriptTags)
        {
            var ltr = new Literal();
            if (addScriptTags)
            {
                ltr.Text = @"<script type=""text/javascript"">//<![CDATA[" + script + @"//]]></script>";
            }
            else
            {
                ltr.Text = script;
            }
            h.Controls.Add(ltr);
        }

    }
}
