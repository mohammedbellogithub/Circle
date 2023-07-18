using Circle.Core.Dtos.Businesses;
using Circle.Core.Repository.Abstraction;
using Circle.Core.ViewModels.Businesses;
using Circle.Shared.Dapper;
using Circle.Shared.Dapper.Interfaces;
using Circle.Shared.Helpers;
using Circle.Shared.Models.Businesses;
using Circle.Shared.Models.UserIdentity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Core.Services.Businesses
{
    public class BusinessService : Service<Business>, IBusinessService
    {
        private readonly UserManager<AppUsers> _userManager;
        private readonly IBusinessRepository _businessRepository;
        public BusinessService(IUnitOfWork unitOfWork, UserManager<AppUsers> userManager, IBusinessRepository businessRepository) : base(unitOfWork)
        {
            _userManager = userManager;
            _businessRepository = businessRepository;
        }

        public async Task CreateBusiness(CreateBusinessViewModel viewModel)
        {
            var business = (Business)viewModel;

            var user = await _userManager.FindByIdAsync(WebHelpers.CurrentUser.UserId);

            if (user is null || user.IsDeleted)
            {

                base.Results.Add(new ValidationResult($"Request failed. Kindly contact technical support."));
                return;
            }

            business.UserAccountId = user.Id;
            business.CreatedBy = WebHelpers.CurrentUser.Email;
            await this.AddAsync(business);
        }

        public async Task<IEnumerable<BusinessResponseViewModel?>> GetUserBusinesses()
        {
            var userId = Guid.Parse(WebHelpers.CurrentUser.UserId);
            var result = await _businessRepository.GetUserBusinessesAsync<BusinessResponseViewModel>(userId);

            return result;
        }

        public async Task UpdateBusiness(EditBusinessViewModel viewModel)
        {
            var userId = Guid.Parse(WebHelpers.CurrentUser.UserId);

            var business = await _businessRepository.GetBusinessByIdAsync<BusinessDto>(viewModel.Id, userId);

            if (business is null)
            {
                base.Results.Add(new ValidationResult($"Business not found, Kindly reconfirm details provided."));
                return;
            }

            var updatedBusiness = (Business)viewModel;
            updatedBusiness.UserAccountId = business.UserAccountId;
            updatedBusiness.CreatedOn = business.CreatedOn;
            updatedBusiness.CreatedBy = business.CreatedBy;
            updatedBusiness.ModifiedBy = WebHelpers.CurrentUser.Email;
            updatedBusiness.ModifiedOn = DateTime.Now;
            updatedBusiness.IsDeleted = business.IsDeleted;
            updatedBusiness.DeletedBy = business.DeletedBy;
            updatedBusiness.DeletedOn = business.DeletedOn;

            await this.UpdateAsync(updatedBusiness);
        }
    }
}
