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
    public class AppUserTokenMap : IEntityTypeConfiguration<AppUserTokens>
    {
        public void Configure(EntityTypeBuilder<AppUserTokens> builder)
        {
            builder.ToTable(nameof(AppUserTokenMap));
            builder.HasKey(b => b.UserId);
        }
    }
}
