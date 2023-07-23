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
    public class BusinessCategoryRepository : Service<BusinessCategory>, IBusinessCategoryRepository
    {
        public BusinessCategoryRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<IEnumerable<T?>> GetUserBusinessCategories<T>(Guid? businessId)
        {
            var userId = Guid.Parse(WebHelpers.CurrentUser.UserId);

            var parameters = new DynamicParameters();
            parameters.Add("businessId", businessId);
            parameters.Add("userId", userId);

            var business = await this.ExecuteStoredProcedure<T>("[sp_get_businessCategoriesByUser]", parameters);

            return business;
        }
    }
}
