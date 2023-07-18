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
        Task<IEnumerable<BusinessResponseViewModel?>> GetUserBusinesses();
        Task CreateBusiness(CreateBusinessViewModel viewModel);
        Task UpdateBusiness(EditBusinessViewModel viewModel);
    }
}
