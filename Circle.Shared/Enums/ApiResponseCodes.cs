using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Shared.Enums
{
    public enum ApiResponseCodes
    {
        [Description("Server error occured, please try again.")]
        EXCEPTION = -5,
        [Description("Unauthorized Access")]
        UNAUTHORIZED = -4,
        NOT_FOUND = -3,
        INVALID_REQUEST = -2,
        [Description("ERROR")]
        ERROR = -1,
        [Description("FAIL")]
        FAIL = 2,
        [Description("SUCCESS")]
        OK = 1,
    }
}
