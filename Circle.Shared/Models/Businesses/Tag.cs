using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Shared.Models.Businesses
{
    public class Tag : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
