using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Shared.Constants
{
    public class MessageTypes
    {
        public const string VerificationMail = "verificationmail";
        public const string PasswordChange = "passwordchange";
    }

    public class MessageSubject
    {
        public const string VerificationMailSubject = "Verify your Circle User registration";
        public const string PasswordchangeSubject = "Your Circle password has been changed";
    }
}
