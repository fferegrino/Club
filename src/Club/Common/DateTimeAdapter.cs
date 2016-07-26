using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Club.Common
{
    public interface IDateTime
    {
        DateTime Now { get; }
    }

    public class DateTimeAdapter : IDateTime
    {
        private readonly DateTime utcDateTime;
        private readonly TimeZoneInfo _timeZone;

        public DateTimeAdapter()
        {
            // Central Standard Time(Mexico)
            var tz = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)");
            _timeZone = tz;
        }
        public DateTime Now => TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
               _timeZone);
    }
}
