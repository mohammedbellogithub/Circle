using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Core.Services.Email
{
    public interface IEmailService
    {
        Task<string> ReadTemplate(string messageType);

        Task SendAsync(string to, string subject, string body);

        Task SendManyAsync(List<string> to, string subject, string body);
    }
}
