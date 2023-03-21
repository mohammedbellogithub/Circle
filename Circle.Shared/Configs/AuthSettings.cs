using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Shared.Configs
{
    public class AuthSettings
    {
        public double TokenExpiry { get; set; }
        public string? Password { get; set; }
        public string? Issuer { get; set; }
        public string? SecretKey { get; set; }
        public bool RequireHttps { get; set; }
    }
}
