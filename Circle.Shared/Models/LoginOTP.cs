using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Shared.Models
{
    public class LoginOTP 
    {
        public string Number { get; set; }
        public string Email { get; set; }
        public DateTime ExpiredOn { get;set; }
        public bool IsVerified { get; set; }

    }
}
