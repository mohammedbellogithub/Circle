using Circle.Shared.Enums;
using Circle.Shared.Models.Businesses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Core.ViewModels.Businesses
{
    public class CreateBusinessViewModel
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public BusinessType BusinessType { get; set; }
        [Required]
        public string? NatureOfBusiness { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? Address3 { get; set; }
        [Required]
        public string? PhoneNumber1 { get; set; }
        public string? PhoneNumber2 { get; set; }
        [Required]
        public string? Email1 { get; set; }
        public string? Email2 { get; set; }

        public static explicit operator Business(CreateBusinessViewModel source)
        {
            var destination = new Business
            {
                Name = source.Name,
                NatureOfBusiness = source.NatureOfBusiness,
                BusinessType = source.BusinessType,
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
