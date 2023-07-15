using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Core.ViewModels
{
    public class LoginOTPViewModel
    {
        public string OTPNumber { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime? ExpiredOn { get; set; }
        public bool IsVerified { get; set; }
    }
}
