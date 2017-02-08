using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace todolist.src
{
    class DateTimeManager
    {
        public static string getUserFriendlyDateTime(DateTime dateTime)
        {
            DateTime now = DateTime.Now;
            TimeSpan diff = new TimeSpan(dateTime.Ticks - now.Ticks);
            string ret = "";

            int nbDays = diff.Days;
            int nbHours = diff.Hours;
            int nbMinutes = diff.Minutes;

            bool isToday = now.DayOfYear == dateTime.DayOfYear && now.Year == dateTime.Year;
            bool isYesterday = now.DayOfYear - 1 == dateTime.DayOfYear && now.Year == dateTime.Year;
            bool isTomorrow = now.DayOfYear + 1 == dateTime.DayOfYear && now.Year == dateTime.Year;

            string tmpHour = dateTime.TimeOfDay.Hours + "";
            string tmpMin = dateTime.TimeOfDay.Minutes + "";
            if (tmpHour.Length == 1)
            {
                tmpHour = "0" + tmpHour;
            }
            if (tmpMin.Length == 1)
            {
                tmpMin = "0" + tmpMin;
            }

            if (nbHours == 0)
            {
                if (nbMinutes == 0)
                {
                    ret = "Less than a minute";
                }
                else if (nbMinutes > 0)
                {
                    ret = nbMinutes + " minutes";
                }
                else if (nbMinutes < 0)
                {
                    ret = "Today, " + tmpHour + ":" + tmpMin;
                }
            }
            else if (isToday)
            {
                ret = "Today, " + tmpHour + ":" + tmpMin;
            }
            else if (isTomorrow)
            {
                ret = "Tomorrow, " + tmpHour + ":" + tmpMin;
            }
            else if (isYesterday)
            {
                ret = "Yesterday, " + tmpHour + ":" + tmpMin;
            }
            else
            {
                ret = dateTime.ToString();
            }
            
            return (ret);
        }
    }
}
