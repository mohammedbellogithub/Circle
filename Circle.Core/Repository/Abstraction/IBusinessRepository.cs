using Circle.Shared.Dapper;
using Circle.Shared.Models.Businesses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Core.Repository.Abstraction
{
    public interface IBusinessRepository : IService<Business>
    {
        IEnumerable<T?> GetExistingBusinessInfo<T,B>(string column, B value);
        Task<T?> GetBusinessByIdAsync<T>(Guid id, Guid userId);
        Task<IEnumerable<T?>> GetUserBusinessesAsync<T>(Guid userId);
    }
}
