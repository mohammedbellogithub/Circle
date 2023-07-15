using Circle.Core.ViewModels;
using Circle.Shared.Dapper;
using Circle.Shared.Dapper.Interfaces;
using Circle.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Core.Services.OTP
{
    public class LoginOtpService : Service<LoginOTP>, ILoginOtpService
    {
        public LoginOtpService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public Task<LoginOTPViewModel> CreateOtp(string email)
        {
            throw new NotImplementedException();
        }
    }
}
