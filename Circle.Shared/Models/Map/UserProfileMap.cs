using Circle.Shared.Models.Statics;
using Circle.Shared.Models.UserIdentity;
using Circle.Shared.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Shared.Models.Map
{
    public class UserProfileMap : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.ToTable(name: nameof(UserProfile));
            SetupData(builder);
        }

        private void SetupData(EntityTypeBuilder<UserProfile> builder)
        {
            var profiles = new UserProfile[]
            {
                new UserProfile
                {
                    Id = Guid.NewGuid(),
                    ProfileName = "CIRCLE",
                    Bio = "The Circle management public profile",
                    ProfilePictureUrl = "https://twitter.com/Mohammed_kingin",
                    BannerPictureUrl = "https://twitter.com/Mohammed_kingin",
                    Location = "Nigria",
                    IsVerified = true,
                    UserAccountId = Defaults.SysUserId
                }
            };
            builder.HasData(profiles);
        }
    }
}
