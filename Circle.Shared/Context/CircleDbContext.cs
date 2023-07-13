using Circle.Shared.Models.Businesses;
using Circle.Shared.Models.Map;
using Circle.Shared.Models.OpenIddict;
using Circle.Shared.Models.UserIdentity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Shared.Context
{
    public class CircleDbContext :  IdentityDbContext<AppUsers, AppRoles, Guid, AppUserClaims, AppUserRoles, AppUserLogins, AppRoleClaims, AppUserTokens>
    {
        public CircleDbContext(DbContextOptions<CircleDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new AppRoleClaimMap());
            modelBuilder.ApplyConfiguration(new AppRoleMap());
            modelBuilder.ApplyConfiguration(new AppUserMap());
            modelBuilder.ApplyConfiguration(new AppUserRoleMap());
            modelBuilder.ApplyConfiguration(new AppUserLoginMap());
            modelBuilder.ApplyConfiguration(new AppUserTokenMap());
            modelBuilder.ApplyConfiguration(new AppUserClaimMap());
            modelBuilder.UseOpenIddict<CircleOpenIddictApplication, CircleOpenIddictAuthorization, CircleOpenIddictScope, CircleOpenIddictToken, Guid>();

            //  var typesToRegister = typeof(BaseEntity).Assembly.GetTypes().Where(type => !String.IsNullOrEmpty(type.Namespace))
            //.Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>));

            //  foreach (var configurationInstance in typesToRegister.Select(Activator.CreateInstance))
            //  {
            //      modelBuilder.ApplyConfiguration((dynamic)configurationInstance);
            //  }

            //  foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            //  {
            //      relationship.DeleteBehavior = DeleteBehavior.Restrict;
            //  }
        }


        public DbSet<Business>? Business { get; set; }
        public DbSet<BusinessCategory>? BusinessCategory { get; set; }


    }
}
