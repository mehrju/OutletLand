using System;
using System.Globalization;

namespace IUtility
{
    public static class PersianDateUtil
    {
        public static string GetPersinNameOfWeekDay(DayOfWeek day)
        {
            try
            {
                switch (day)
                {
                    case DayOfWeek.Saturday:
                        return "شنبه".ToPersianContent();
                    case DayOfWeek.Sunday:
                        return "یکشنبه".ToPersianContent();
                    case DayOfWeek.Monday:
                        return "دوشنبه".ToPersianContent();
                    case DayOfWeek.Tuesday:
                        return "سه شنبه".ToPersianContent();
                    case DayOfWeek.Wednesday:
                        return "چهارشنبه".ToPersianContent();
                    case DayOfWeek.Thursday:
                        return "پنج شنبه".ToPersianContent();
                    case DayOfWeek.Friday:
                        return "جمعه".ToPersianContent();
                }
                return "";
            }
            catch (Exception ex)
            {
                return string.Format("روز نا معتبر به دلیل : {0}",ex.Message);
            }
        }
        public static string GetPersinNameOfMount(int mount)
        {
            try
            {
                switch (mount)
                {
                    case 1:
                        return "فروردین".ToPersianContent();
                    case 2:
                        return "اردیبهشت".ToPersianContent();
                    case 3:
                        return "خرداد".ToPersianContent();
                    case 4:
                        return "تیر".ToPersianContent();
                    case 5:
                        return "مرداد".ToPersianContent();
                    case 6:
                        return "شهریور".ToPersianContent();
                    case 7:
                        return "مهر".ToPersianContent();
                    case 8:
                        return "آبان".ToPersianContent();
                    case 9:
                        return "آذر".ToPersianContent();
                    case 10:
                        return "دی".ToPersianContent();
                    case 11:
                        return "بهمن".ToPersianContent();
                    case 12:
                        return "اسفند".ToPersianContent();
                }
                return "";
            }
            catch (Exception ex)
            {
                return string.Format("ماه نا معتبر به دلیل : {0}", ex.Message);
            }
        }
        public static string GetPersinNameOfDay(int day)
        {
            try
            {
                switch (day)
                {
                    case 1:
                        return "یکم".ToPersianContent();
                    case 2:
                        return "دوم".ToPersianContent();
                    case 3:
                        return "سوم".ToPersianContent();
                    case 4:
                        return "چهارم".ToPersianContent();
                    case 5:
                        return "پنجم".ToPersianContent();
                    case 6:
                        return "ششم".ToPersianContent();
                    case 7:
                        return "هفتم".ToPersianContent();
                    case 8:
                        return "هشتم".ToPersianContent();
                    case 9:
                        return "نهم".ToPersianContent();
                    case 10:
                        return "دهم".ToPersianContent();
                    case 11:
                        return "یازدهم".ToPersianContent();
                    case 12:
                        return "دوازدهم".ToPersianContent();
                    case 13:
                        return "سیزدهم".ToPersianContent();
                    case 14:
                        return "چهاردهم".ToPersianContent();
                    case 15:
                        return "پانزدهم".ToPersianContent();
                    case 16:
                        return "شانزدهم".ToPersianContent();
                    case 17:
                        return "هفدهم".ToPersianContent();
                    case 18:
                        return "هجدهم".ToPersianContent();
                    case 19:
                        return "نوزدهم".ToPersianContent();
                    case 20:
                        return "بیستم".ToPersianContent();
                    case 21:
                        return "بیست و یکم".ToPersianContent();
                    case 22:
                        return "بیست و دوم".ToPersianContent();
                    case 23:
                        return "بیست و سوم".ToPersianContent();
                    case 24:
                        return "بیست و چهارم".ToPersianContent();
                    case 25:
                        return "بیست و پنجم".ToPersianContent();
                    case 26:
                        return "بیست و ششم".ToPersianContent();
                    case 27:
                        return "بیست و هفتم".ToPersianContent();
                    case 28:
                        return "بیست و هشتم".ToPersianContent();
                    case 29:
                        return "بیست و نهم".ToPersianContent();
                    case 30:
                        return "سی ام".ToPersianContent();
                    case 31:
                        return "سی و یکم".ToPersianContent();
                }
                return "";
            }
            catch (Exception ex)
            {
                return string.Format("روز ماه نا معتبر به دلیل : {0}", ex.Message);
            }
        }
        public static string GetShortPersianDateWithDash(this DateTime dateTime)
        {
            try
            {
                return (new PersianCalendar().GetYear(dateTime) + "-" +
                        new PersianCalendar().GetMonth(dateTime) + "-" +
                        new PersianCalendar().GetDayOfMonth(dateTime) + "  -  " +
                        new PersianCalendar().GetHour(dateTime) + "-" +
                        new PersianCalendar().GetMinute(dateTime));
            }
            catch (Exception ex)
            {
                return string.Format("تاریخ نا معتبر به دلیل : {0}", ex.Message);
            }

        }
        public static string GetShortPersianDate(this DateTime dateTime)
        {
            try
            {
                //return (new PersianCalendar().GetYear(dateTime).ToString().ToPersianNumbers() + "/" +
                //        new PersianCalendar().GetMonth(dateTime).ToString().ToPersianNumbers() + "/" +
                //        new PersianCalendar().GetDayOfMonth(dateTime).ToString().ToPersianNumbers());
                return (new PersianCalendar().GetYear(dateTime) + "/" +
                        new PersianCalendar().GetMonth(dateTime) + "/" +
                        new PersianCalendar().GetDayOfMonth(dateTime));
            }
            catch (Exception ex)
            {
                return string.Format("تاریخ نا معتبر به دلیل : {0}", ex.Message);
            }

        }
        public static string GetShortPersianDateTime(this DateTime dateTime)
        {
            try
            {
                //return (new PersianCalendar().GetYear(dateTime).ToString().ToPersianNumbers() + "/" +
                //        new PersianCalendar().GetMonth(dateTime).ToString().ToPersianNumbers() + "/" +
                //        new PersianCalendar().GetDayOfMonth(dateTime).ToString().ToPersianNumbers());
                return (new PersianCalendar().GetYear(dateTime) + "/" +
                        new PersianCalendar().GetMonth(dateTime) + "/" +
                        new PersianCalendar().GetDayOfMonth(dateTime) + " - " +
                        new PersianCalendar().GetHour(dateTime) + ":" +
                        new PersianCalendar().GetMinute(dateTime));
            }
            catch (Exception ex)
            {
                return string.Format("تاریخ نا معتبر به دلیل : {0}", ex.Message);
            }

        }
        public static string GetLongPersianDate(this DateTime dateTime)
        {
            try
            {
                return (GetPersinNameOfWeekDay(new PersianCalendar().GetDayOfWeek(dateTime)) + "، " +
                        GetPersinNameOfDay(new PersianCalendar().GetDayOfMonth(dateTime)) + "  " +
                        GetPersinNameOfMount(new PersianCalendar().GetMonth(dateTime)) + "  " +
                        (new PersianCalendar().GetYear(dateTime).ToString(CultureInfo.InvariantCulture).ToPersianNumbers()));
            }
            catch (Exception ex)
            {
                return string.Format("تاریخ نا معتبر به دلیل : {0}", ex.Message);
            }
        }
        public static string GetLongPersianDateTime(this DateTime dateTime)
        {
            try
            {
                return (GetPersinNameOfWeekDay(new PersianCalendar().GetDayOfWeek(dateTime)) + "، " +
                        GetPersinNameOfDay(new PersianCalendar().GetDayOfMonth(dateTime)) + "  " +
                        GetPersinNameOfMount(new PersianCalendar().GetMonth(dateTime)) + " ماه  " +
                        (new PersianCalendar().GetYear(dateTime).ToString(CultureInfo.InvariantCulture).ToPersianNumbers())) +
                        " - ساعت " +
                        new PersianCalendar().GetHour(dateTime).ToString(CultureInfo.InvariantCulture).ToPersianNumbers()
                        + ":" +
                        new PersianCalendar().GetMinute(dateTime).ToString(CultureInfo.InvariantCulture).ToPersianNumbers();
            }
            catch (Exception ex)
            {
                return string.Format("تاریخ نا معتبر به دلیل : {0}", ex.Message);
            }
        }

        public static DateTime GetMiladiDate(this string persianDate)
        {
            string[] strDate = persianDate.Split('/');
            int y, m, d;
            if (strDate[0].Length == 4)
            {
                y = int.Parse(strDate[0]);
                m = int.Parse(strDate[1]);
                d = int.Parse(strDate[2]);
            }
            else
            {
                y = int.Parse(strDate[2]);
                m = int.Parse(strDate[1]);
                d = int.Parse(strDate[0]);
            }
            var jC = new PersianCalendar();
            return jC.ToDateTime(y, m, d, 0, 0, 0, 0);
        }
    }
}
