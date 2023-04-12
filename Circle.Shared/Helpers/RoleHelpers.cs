using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Shared.Helpers
{
    public static class RoleHelpers
    {
        public static Guid SYS_ADMIN_ID() => Guid.Parse("773a3af2-cd9f-4f65-869f-0cfdc1e1589e");
        public const string SYS_ADMIN = nameof(SYS_ADMIN);

        public static Guid ADMIN_ID() => Guid.Parse("cc785f2a-2c0a-4648-87b7-a500084a2c1a");
        public const string ADMIN = nameof(ADMIN);

        public static Guid FRONTDESK_ID() => Guid.Parse("ca7061a2-138c-45b7-870c-699caa9ca99b");
        public const string FRONTDESK = nameof(FRONTDESK);

        public static Guid DEFAULT_ID() => Guid.Parse("cf185b00-652d-4c52-a3fb-4c94cb794718");
        public const string DEFAULT = nameof(DEFAULT);



        public static List<string> GetAll()
        {
            return new List<string>
            {
                SYS_ADMIN,
                ADMIN,
                DEFAULT,
                //HR,
                //ACCOUNTANT,
                //ACADEMY,
                FRONTDESK
                //SALES
            };
        }
    }
}
