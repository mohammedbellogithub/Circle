using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Circle.Shared.Models.Businesses
{
    public class BusinessCategory : BaseEntity
    {
        public string? Name { get; set; }
        public string? UniqueIdentiferCode { get; set; }
        public string? Description { get; set; }
        public Guid? ParentBusinessCategoryId { get; set; }
        public ICollection<BusinessListing> BusinessListing { get; set; } = new List<BusinessListing>();
        [ForeignKey(nameof(Business))]
        public Guid BusinessId { get; set; }
        public Business Business { get; set; }

    }
}
