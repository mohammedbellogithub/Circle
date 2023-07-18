using Circle.Core.Dtos.User;
using Circle.Shared.Enums;
using Circle.Shared.Models.Businesses;
using Circle.Shared.Models.UserIdentity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Core.Dtos.Businesses
{
    public class BusinessDto : BaseDto
    {
        public string? Name { get; set; }
        public BusinessType BusinessType { get; set; }
        public string? NatureOfBusiness { get; set; }
        public string? Description { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? Address3 { get; set; }
        public string? PhoneNumber1 { get; set; }
        public string? PhoneNumber2 { get; set; }
        public string? Email1 { get; set; }
        public string? Email2 { get; set; }
        public bool IsActive { get; set; }
        public bool Verified { get; set; }
        public ICollection<BusinessCategoryDto> BusinessCategoryDto { get; } = new List<BusinessCategoryDto>();

        public Guid UserAccountId { get; set; }
        public UserDto AppUsers { get; set; }

    }
}
