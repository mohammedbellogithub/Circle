using Circle.Shared.Models.Businesses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Core.ViewModels.Businesses
{
    public class BusinessCategoryViewModel
    {
        public string? Name { get; set; }
        public string? UniqueIdentiferCode { get; set; }
        public string? Description { get; set; }
        public Guid? ParentBusinessCategoryId { get; set; }
        public List<BusinessListing>? BusinessListing { get; set; } 
        public Guid BusinessId { get; set; }
        public Business Business { get; set; }
    }

    public class CreateBusinessCategoryViewModel
    {
        public string? Name { get; set; }
        public string? UniqueIdentifierCode { get; set; }
        public string? Description { get; set; }
        public Guid BusinessId { get; set; }

        public static explicit operator BusinessCategory(CreateBusinessCategoryViewModel source)
        {
            var destination = new BusinessCategory
            {
                Name = source.Name,
                UniqueIdentiferCode = source.UniqueIdentifierCode,
                Description = source.Description,
                BusinessId = source.BusinessId,
            };
            return destination;
        }
    }

    public class CreateBusinessSubCategoryViewModel
    {
        public string? Name { get; set; }
        public string? UniqueIdentifierCode { get; set; }
        public string? Description { get; set; }
        public Guid ParentBusinessCategoryId { get; set; }
    }

    public class UpdateBusinessCategoryViewModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? UniqueIdentifierCode { get; set; }
        public string? Description { get; set; }
    }
}
