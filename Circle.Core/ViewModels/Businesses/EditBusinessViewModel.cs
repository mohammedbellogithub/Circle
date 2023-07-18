using Circle.Shared.Enums;
using Circle.Shared.Models.Businesses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Core.ViewModels.Businesses
{
    public class EditBusinessViewModel
    {
        public Guid Id { get; set; }
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

        public static explicit operator Business(EditBusinessViewModel source)
        {
            var destination = new Business()
            {
                Id = source.Id,
                Name = source.Name,
                BusinessType = source.BusinessType,
                NatureOfBusiness = source.NatureOfBusiness,
                Description = source.Description,
                Address1 = source.Address1,
                Address2 = source.Address2,
                Address3 = source.Address3,
                PhoneNumber1 = source.PhoneNumber1,
                PhoneNumber2 = source.PhoneNumber2,
                Email1 = source.Email1,
                Email2 = source.Email2,
            };
            return destination;
        }
    }
}
