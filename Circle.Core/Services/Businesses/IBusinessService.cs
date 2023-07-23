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
    public interface IBusinessService : IService<Business>
    {
        Task<BusinessResponseViewModel?> GetBusinessDetail(Guid id);
        Task<IEnumerable<BusinessResponseViewModel?>> GetUserBusinesses();
        Task CreateBusinessAsync(CreateBusinessViewModel viewModel);
        Task UpdateBusinessAsync(EditBusinessViewModel viewModel);
        Task DeleteBusinessAsync(Guid id);
    }
}
