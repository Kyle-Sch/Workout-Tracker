using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;

namespace WebApplication.Web.Providers
{
    public static class VisitCalculator
    {
        public static TimeSpan GetAvgVisit(List<Visit> visits)
        {
            int days = 0;
            int hours = 0;
            int mins = 0;
            int seconds = 0;
            if (visits != null && visits.Count > 0)
            {
                foreach (Visit visit in visits)
                {
                    TimeSpan timespan = visit.Departure.Subtract(visit.Arrival);
                    days += timespan.Days;
                    hours += timespan.Hours;
                    mins += timespan.Minutes;
                    seconds += timespan.Seconds;
                }
                double remainder = ((double)days / visits.Count) % 1;
                days = days / visits.Count;
                double convertedRemainder = remainder * 24;

                remainder = ((hours + convertedRemainder % 1) / visits.Count) % 1;
                hours = hours / visits.Count + +(int)Math.Floor(convertedRemainder);
                convertedRemainder = remainder * 60;

                remainder = ((mins + convertedRemainder % 1) / visits.Count) % 1;
                mins = (mins) / visits.Count + (int)Math.Floor(convertedRemainder);
                convertedRemainder = remainder * 60;

                seconds = (seconds) / visits.Count + (int)Math.Floor(convertedRemainder);
            }
            TimeSpan output = new TimeSpan(days, hours, mins, seconds);
            return output;
        }
    }
}
