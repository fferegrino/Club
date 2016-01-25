using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Club.Services
{
    public interface IMailService
    {
        Task<bool> SendMail(string to, string from, string subject, string body);
    }
}
