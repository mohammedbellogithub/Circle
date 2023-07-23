using Circle.Core.Repository.Abstraction;
using Circle.Shared.Dapper;
using Circle.Shared.Dapper.Interfaces;
using Circle.Shared.Helpers;
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

        public async Task<T?> GetBusinessByIdAsync<T>(Guid id)
        {
            var userId = Guid.Parse(WebHelpers.CurrentUser.UserId);

            var parameters = new DynamicParameters();
            parameters.Add("Id", id);
            parameters.Add("userId", userId);

            var business = await this.ExecuteStoredProcedure<T>("[sp_get_businessById]", parameters);

            return business.FirstOrDefault();
        }

        public async Task<IEnumerable<T?>> GetUserBusinessesAsync<T>()
        {
            var userId = Guid.Parse(WebHelpers.CurrentUser.UserId);

            var parameters = new DynamicParameters();
            parameters.Add("userId", userId);

            var businesses = await this.ExecuteStoredProcedure<T>("[sp_get_userBusinesses]", parameters);

            return businesses;
        }
        
        public IEnumerable<T?> GetExistingBusinessInfo<T,B>(string column, B value)
        {

            var department = this.SqlQuery<T>(@$"SELECT * FROM [Business] WHERE {column} = @value AND IsDeleted <> 1",
                new
                {
                    value = value
                }); ;


            return department;
        }
    }
}
