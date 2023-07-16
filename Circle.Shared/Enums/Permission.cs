using Circle.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Shared.Enums
{
    public enum Permission
    {
        [Category(RoleHelpers.SYS_ADMIN), Description(@"Access All Modules")]
        FULL_CONTROL = 1001,
        [Category(RoleHelpers.DEFAULT), Description(@"Access All Default User Modules")]
        FULL_DEFAULT_USER_CONTROL = 2001,
        [Category(RoleHelpers.FRONTDESK), Description(@"Access user support module.")]
        FRONTDESK_CONTROL = 3001,
        [Category(RoleHelpers.ADMIN), Description(@"Access user support module.")]
        FULL_USER_CONTROL = 4001,
    }
}
