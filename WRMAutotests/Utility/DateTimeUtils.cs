using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WRMAutotests.Utility
{
    public class DateTimeUtils
    {

        public static TimeZoneInfo GetTimezoneInfoByPartOfIdOfTimezone(String partOfId)
        {
            ReadOnlyCollection<TimeZoneInfo> tz;
            tz = TimeZoneInfo.GetSystemTimeZones();
            foreach (TimeZoneInfo tzInfo in tz)
            {
                if (tzInfo.ToString().ToLower().Contains(partOfId.ToLower()))
                    return tzInfo;
            }
            throw new AssertionException("Absent timezone with part: " + partOfId);
        }

        

    }
}
