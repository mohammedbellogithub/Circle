using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Core.Dtos.User
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? RoleName { get; set; }
        public Guid? RoleId { get; set; }
        public bool Activated { get; set; }
        public bool IsPasswordDefault { get; set; }
        public string? StaffNo { get; set; }
        public bool LockoutEnabled { get; set; }
        public string? Department { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public int Gender { get; set; }

        public int TotalCount { get; set; }
    }
}
