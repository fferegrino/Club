using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Club.Common
{
    public interface IDateTime
    {
        DateTime UtcNow { get; }
    }

    public class DateTimeAdapter : IDateTime
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
