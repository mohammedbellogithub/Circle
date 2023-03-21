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
    public class AppUserClaimMap : IEntityTypeConfiguration<AppUserClaims>
    {
        public void Configure(EntityTypeBuilder<AppUserClaims> builder)
        {
            builder.ToTable(nameof(AppUserClaims));
            builder.HasKey(c => c.Id);
        }
    }
}
