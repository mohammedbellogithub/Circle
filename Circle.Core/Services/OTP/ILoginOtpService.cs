using Circle.Core.ViewModels;
using Circle.Shared.Dapper;
using Circle.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Core.Services.OTP
{
    public interface ILoginOtpService : IService<LoginOTP>
    {
        Task<LoginOTPViewModel> CreateOtp(string email);
    }
}
