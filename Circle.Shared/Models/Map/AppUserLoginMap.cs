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
    public class AppUserLoginMap : IEntityTypeConfiguration<AppUserLogins>
    {
        public void Configure(EntityTypeBuilder<AppUserLogins> builder)
        {
            builder.ToTable(nameof(AppUserLogins));
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.HasKey(u => new { u.LoginProvider, u.ProviderKey });
        }
    }
}
