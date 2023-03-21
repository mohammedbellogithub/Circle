
using Circle.Shared.Helpers;
using Circle.Shared.Models.Statics;
using Circle.Shared.Models.UserIdentity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Shared.Models.Map
{
    public class AppUserRoleMap : IEntityTypeConfiguration<AppUserRoles>
    {
        public void Configure(EntityTypeBuilder<AppUserRoles> builder)
        {
            builder.ToTable(name: nameof(AppUserRoles));
            builder.HasKey(p => new { p.UserId, p.RoleId });
            SetupData(builder);
        }

        private void SetupData(EntityTypeBuilder<AppUserRoles> builder)
        {
            List<AppUserRoles> dataList = new List<AppUserRoles>()
            {
                new AppUserRoles()
                {
                    UserId = Defaults.SysUserId,
                    RoleId = RoleHelpers.SYS_ADMIN_ID(),
                },

                new AppUserRoles()
                {
                    UserId = Defaults.SuperAdminId,
                    RoleId = RoleHelpers.FRONTDESK_ID(),
                },
                 new AppUserRoles()
                {
                    UserId = Defaults.AdminId,
                    RoleId = RoleHelpers.ADMIN_ID(),
                }
            };

            builder.HasData(dataList);
        }
    }
}
