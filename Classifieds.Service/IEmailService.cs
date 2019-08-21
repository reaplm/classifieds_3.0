using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Classifieds.Service
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
