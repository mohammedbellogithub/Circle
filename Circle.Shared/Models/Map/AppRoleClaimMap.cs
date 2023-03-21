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
    public class AppRoleClaimMap : IEntityTypeConfiguration<AppRoleClaims>
    {
        public void Configure(EntityTypeBuilder<AppRoleClaims> builder)
        {
            builder.ToTable(nameof(AppRoleClaims));
            builder.HasKey(p => p.Id);
        }
    }
}
