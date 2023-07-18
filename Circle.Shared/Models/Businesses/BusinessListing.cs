using Circle.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Shared.Models.Businesses
{
    public class BusinessListing : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public ICollection<BusinessListingDocument> BusinessListingDocuments { get; set; } = new List<BusinessListingDocument>();
        public BusinessListingStatus Status { get; set; }
        public int? Unit { get; set; }
        public int Ratings { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public bool Discounted { get; set; }
        [ForeignKey(nameof(BusinessCategory))]
        public Guid BusinessCategoryId { get; set; }
        public BusinessCategory BusinessCategory { get; set;}
    }

    public class BusinessListingDocument : BaseEntity
    {
        public string? Path { get; set; }
        public string? ContentType { get; set; }
        public long FileLength { get; set; }
        public string? OriginalFileName { get; set; }
        public string? Name { get; set; }
        public BusinessListingDocumentType DocumentType { get; set; }
        [ForeignKey(nameof(BusinessListing))]
        public Guid BusinessListingId { get; set; }
        public BusinessListing BusinessListing { get; set; }

    }


}
