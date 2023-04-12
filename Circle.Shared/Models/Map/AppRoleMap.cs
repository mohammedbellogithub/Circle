using Circle.Shared.Helpers;
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
    public class AppRoleMap : IEntityTypeConfiguration<AppRoles>
    {
        public void Configure(EntityTypeBuilder<AppRoles> builder)
        {
            builder.ToTable(name: nameof(AppRoles));
            SetupData(builder);
        }

        private void SetupData(EntityTypeBuilder<AppRoles> builder)
        {
            var roles = new AppRoles[]
            {
                new AppRoles
                {
                    Id = RoleHelpers.SYS_ADMIN_ID(),
                    Name = RoleHelpers.SYS_ADMIN,
                    NormalizedName = RoleHelpers.SYS_ADMIN.ToString(),
                    IsInBuilt = true
                },
                 new AppRoles
                {
                    Id = RoleHelpers.ADMIN_ID(),
                    Name = RoleHelpers.ADMIN,
                    NormalizedName = RoleHelpers.ADMIN.ToString(),
                    IsInBuilt = true
                },
                  new AppRoles
                {
                    Id = RoleHelpers.FRONTDESK_ID(),
                    Name = RoleHelpers.FRONTDESK,
                    NormalizedName = RoleHelpers.FRONTDESK.ToString(),
                    IsInBuilt = true
                },
                  new AppRoles
                {
                    Id = RoleHelpers.DEFAULT_ID(),
                    Name = RoleHelpers.DEFAULT,
                    NormalizedName = RoleHelpers.DEFAULT.ToString(),
                    IsInBuilt = true
                }
            };

            builder.HasData(roles);
        }
    }
}
