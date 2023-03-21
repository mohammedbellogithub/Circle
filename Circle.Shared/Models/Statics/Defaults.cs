using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Shared.Models.Statics
{
    public partial class Defaults
    {
        public const string SysUserEmail = "system@innercircle.com";
        public static readonly Guid SysUserId = Guid.Parse("50b70c44-9eb7-4549-9a48-7d37809b7d8e");
        public const string SysUserMobile = "08108565760";
        //
        public const string SuperAdminEmail = "mohammedbello678@gmail.com";
        public static readonly Guid SuperAdminId = Guid.Parse("1743b5bd-1eb1-45b3-9630-99596b17cf53");
        public const string SuperAdminMobile = "09025055210";
        //
        public static Guid AdminId = Guid.Parse("ca5eb7a4-de1e-40a1-9c58-ac452112aa92");
        public const string AdminEmail = "admin@innercircle.com";
        public const string AdminMobile = "09025055210";
        //
        public static readonly Guid FrontDeskId = Guid.Parse("96623538-0615-4d01-9023-7352bb4bb9c6");
        public const string FrontDeskMobile = "+2349025055210";
        public const string FrontDeskEmail = "frontdesk@innercircle.com";
    }
}
