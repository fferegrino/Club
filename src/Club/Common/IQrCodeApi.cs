using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Club.Common
{
    public interface IQrCodeApi
    {
        string GetQrUrl(string eventAttendanceAddress, int size);
    }
}
