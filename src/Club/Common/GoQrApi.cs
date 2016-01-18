using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Club.Common
{
    public class GoQrApi : IQrCodeApi
    {
        public string GetQrUrl(string eventAttendanceAddress, int size)
        {
            var escapedAddress = System.Uri.EscapeDataString(eventAttendanceAddress);
            return $"//api.qrserver.com/v1/create-qr-code/?data={escapedAddress}&size={size}x{size}";
        }
    }
}
