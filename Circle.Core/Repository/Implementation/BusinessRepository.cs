using Circle.Core.Repository.Abstraction;
using Circle.Shared.Dapper;
using Circle.Shared.Dapper.Interfaces;
using Circle.Shared.Models.Businesses;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Core.Repository.Implementation
{
    public class BusinessRepository : Service<Business>, IBusinessRepository
    {
        public BusinessRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<T?> GetBusinessByIdAsync<T>(Guid id, Guid userId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("Id", id);
            parameters.Add("userId", userId);

            var business = await this.ExecuteStoredProcedure<T>("[sp_get_businessById]", parameters);

            return business.FirstOrDefault();
        }

        public async Task<IEnumerable<T?>> GetUserBusinessesAsync<T>(Guid userId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("userId", userId);

            var businesses = await this.ExecuteStoredProcedure<T>("[sp_get_userBusinesses]", parameters);

            return businesses;
        }
    }
}
