using Circle.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Shared.Models.Businesses
{
    public class Business : BaseEntity
    {
        public string? Name { get; set; }
        public BusinessType BusinessType { get; set; }
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

    }
}
