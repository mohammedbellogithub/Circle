using Circle.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Shared.Models.Businesses
{
    public class BusinessListing : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public ICollection<BusinessListingDocument> BusinessListingDocuments { get; set; } = new Collection<BusinessListingDocument>();
        public BusinessListingStatus Status { get; set; }
        public int? Unit { get; set; }
        public int Ratings { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public bool Discounted { get; set; }
        public Guid BusinessCategoryId { get; set; }
    }

    public class BusinessListingDocument : BaseEntity
    {
        public string? Path { get; set; }
        public string? ContentType { get; set; }
        public long FileLength { get; set; }
        public string? OriginalFileName { get; set; }
        public string? Name { get; set; }
        public BusinessListingDocumentType DocumentType { get; set; }
        public Guid BusinessListingId { get; set; }

    }


}
