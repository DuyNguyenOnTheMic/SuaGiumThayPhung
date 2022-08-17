using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPTimeTableIT.Infrastructure.Services.Abstract
{
    public interface ICapstoneEmailService
    {
        void SendMail(string to, string subject, string content);
        void SendHtmlMail(string to, string subject, string content);
    }
}
