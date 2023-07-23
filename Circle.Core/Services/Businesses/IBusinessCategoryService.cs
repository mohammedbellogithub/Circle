using Circle.Core.ViewModels.Businesses;
using Circle.Shared.Dapper;
using Circle.Shared.Models.Businesses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Core.Services.Businesses
{
    public interface IBusinessCategoryService : IService<BusinessCategory>
    {
        Task CreateBusinessCategory(CreateBusinessCategoryViewModel viewModel);
        Task EditBusinessCategory(UpdateBusinessCategoryViewModel viewModel);
        Task DeleteBusinessCategory(Guid id);  
        Task<IEnumerable<BusinessCategoryViewModel?>> GetAllBusinessCategories(Guid BusinessId);
    }
}
