using Circle.Shared.Dapper;
using Circle.Shared.Models.Businesses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Core.Repository.Abstraction
{
    public interface IBusinessCategoryRepository : IService<BusinessCategory>
    {
        Task<IEnumerable<T?>> GetUserBusinessCategories<T>(Guid? businessId);
    }

}
