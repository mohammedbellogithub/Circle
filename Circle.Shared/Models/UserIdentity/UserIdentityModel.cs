using Circle.Shared.Utils;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Shared.Models.UserIdentity
{
    public class AppUsers : IdentityUser<Guid>, IEntity
    {
        public AppUsers()
        {
            Id = SequentialGuidGenerator.Instance.Create();
            CreatedOn = DateTime.UtcNow;
        }

        public string? RefreshToken { get; set; }
        public string? ProviderKey { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? Unit { get; set; }
        public int? Gender { get; set; }
        public int? UserType { get; set; }
        public Guid? UserTypeId { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public bool Activated { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string? Department { get; set; }
        public bool? IsPasswordDefault { get; set; }
        public string? StaffNo { get; set; }
    }

    public class AppUserClaims : IdentityUserClaim<Guid>
    {
    }

    public class AppUserLogins : IdentityUserLogin<Guid>
    {
        [Key]
        [Required]
        public int Id { get; set; }
    }

    public class AppRoles : IdentityRole<Guid>
    {
        public AppRoles()
        {
            Id = Guid.NewGuid();
            ConcurrencyStamp = Guid.NewGuid().ToString("N");
        }

        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool? IsInBuilt { get; set; } = false;
    }

    public class AppUserRoles : IdentityUserRole<Guid>
    {
    }

    public class AppRoleClaims : IdentityRoleClaim<Guid>
    {
    }

    public class AppUserTokens : IdentityUserToken<Guid>
    {
    }
}
