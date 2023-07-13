using System.Collections.ObjectModel;

namespace Circle.Shared.Models.Businesses
{
    public class BusinessCategory : BaseEntity
    {
        public string? Name { get; set; }
        public string? UniqueIdentiferCode { get; set; }
        public string? Description { get; set; }
        public Guid? ParentBusinessCategoryId { get; set; }
        public ICollection<BusinessListing> BusinessListingDocuments { get; set; } = new Collection<BusinessListing>();
        public Guid BusinessId { get; set; }
        public virtual Business Business { get; set; }

    }
}
