using Circle.Shared.Extensions;
using Circle.Shared.Models.UserIdentity;
using Circle.Shared.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Shared.Models.Users
{
    public class UserProfile
    {
        public Guid Id { get; set; }
        public string? ProfileName { get; set; }
        public string? Bio { get; set; }
        public string? Location { get; set; }
        public bool IsVerified { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? BannerPictureUrl { get; set; }
        [ForeignKey(nameof(AppUsers))]
        public Guid UserAccountId { get; set; }
        public AppUsers? UserAccount { get; set; }

        public UserProfile()
        {
            Id = SequentialGuidGenerator.Instance.Create();
        }

    }
}
