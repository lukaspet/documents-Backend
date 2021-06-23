using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi10Min.Models;

namespace WebApi10Min.Helpers.Email
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
