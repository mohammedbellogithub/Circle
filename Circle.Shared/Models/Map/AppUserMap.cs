using Circle.Shared.Models.Statics;
using Circle.Shared.Models.UserIdentity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Shared.Models.Map
{
    public class AppUserMap : IEntityTypeConfiguration<AppUsers>
    {
        public void Configure(EntityTypeBuilder<AppUsers> builder)
        {
            builder.ToTable(name: nameof(AppUsers));
            SetupSuperAdmin(builder);
            SetupAdmin(builder);
            SetupFrontDesk(builder);
        }

        public PasswordHasher<AppUsers> Hasher { get; set; } = new PasswordHasher<AppUsers>();

        private void SetupSuperAdmin(EntityTypeBuilder<AppUsers> builder)
        {
            var sysUser = new AppUsers
            {
                Activated = true,
                CreatedOn = new DateTime(2022, 10, 15),
                FirstName = "John",
                LastName = "Doe",
                Id = Defaults.SysUserId,
                LastLoginDate = new DateTime(2022, 10, 15),
                Email = Defaults.SysUserEmail,
                EmailConfirmed = true,
                NormalizedEmail = Defaults.SysUserEmail.ToUpper(),
                PhoneNumber = Defaults.SysUserMobile,
                UserName = Defaults.SysUserEmail,
                NormalizedUserName = Defaults.SysUserEmail.ToUpper(),
                TwoFactorEnabled = false,
                PhoneNumberConfirmed = true,
                PasswordHash = Hasher.HashPassword(null, "grantAccess"),
                SecurityStamp = "3c147856-b944-49f7-8c03-86eab5feadac",
            };

            var superUser = new AppUsers
            {
                Activated = true,
                CreatedOn = new DateTime(2022, 10, 15),
                Id = Defaults.SuperAdminId,
                FirstName = "Mohammed",
                LastName = "Bello",
                LastLoginDate = new DateTime(2022, 10, 15),
                Email = Defaults.SuperAdminEmail,
                EmailConfirmed = true,
                NormalizedEmail = Defaults.SuperAdminEmail.ToUpper(),
                PhoneNumber = Defaults.SuperAdminMobile,
                UserName = Defaults.SuperAdminEmail,
                NormalizedUserName = Defaults.SuperAdminEmail.ToUpper(),
                TwoFactorEnabled = false,
                PhoneNumberConfirmed = true,
                PasswordHash = Hasher.HashPassword(null, "grantAccess"),
                SecurityStamp = "318338a4-8f26-47d7-bb01-66b8784aeae6",
            };

            builder.HasData(sysUser, superUser);
        }

        private void SetupAdmin(EntityTypeBuilder<AppUsers> builder)
        {
            var adminUser = new AppUsers
            {
                Activated = true,
                CreatedOn = new DateTime(2022, 10, 15),
                FirstName = "",
                LastName = "Admin",
                Id = Defaults.AdminId,
                LastLoginDate = new DateTime(2022, 10, 15),
                Email = Defaults.AdminEmail,
                EmailConfirmed = true,
                NormalizedEmail = Defaults.AdminEmail.ToUpper(),
                PhoneNumber = Defaults.AdminMobile,
                UserName = Defaults.AdminEmail,
                NormalizedUserName = Defaults.AdminEmail.ToUpper(),
                TwoFactorEnabled = false,
                PhoneNumberConfirmed = true,
                PasswordHash = Hasher.HashPassword(null, "grantAccess"),
                SecurityStamp = "d2db0156-280e-4867-9795-8303362024dd",
            };

            builder.HasData(adminUser);
        }

        private void SetupFrontDesk(EntityTypeBuilder<AppUsers> builder)
        {
            var frontdeskUser = new AppUsers
            {
                Activated = true,
                CreatedOn = new DateTime(2020, 10, 15),
                FirstName = "babatunde",
                LastName = "Bello",
                Id = Defaults.FrontDeskId,
                LastLoginDate = new DateTime(2020, 10, 15),
                Email = Defaults.FrontDeskEmail,
                EmailConfirmed = true,
                NormalizedEmail = Defaults.FrontDeskEmail.ToUpper(),
                PhoneNumber = Defaults.FrontDeskMobile,
                UserName = Defaults.FrontDeskEmail,
                NormalizedUserName = Defaults.FrontDeskEmail.ToUpper(),
                TwoFactorEnabled = false,
                PhoneNumberConfirmed = true,
                PasswordHash = Hasher.HashPassword(null, "grantAccess"),
                SecurityStamp = "81b94cda-96bb-43e0-ac86-6d4a3de474f9",
            };

            builder.HasData(frontdeskUser);
        }
    }
}
