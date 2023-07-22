using Circle.Core.Dtos.Businesses;
using Circle.Core.Repository.Abstraction;
using Circle.Core.ViewModels.Businesses;
using Circle.Shared.Dapper;
using Circle.Shared.Dapper.Interfaces;
using Circle.Shared.Extensions;
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
            if (!viewModel.Email1.IsValidEmail() || !viewModel.Email2.IsValidEmail())
            {
                base.Results.Add(new ValidationResult($"Email format is invalid."));
                return;
            }

            if (!viewModel.PhoneNumber1.IsValidPhoneNumber() || !viewModel.PhoneNumber2.IsValidPhoneNumber())
            {
                base.Results.Add(new ValidationResult($"Phone number format is invalid."));
                return;
            }

            var user = await _userManager.FindByIdAsync(WebHelpers.CurrentUser.UserId);

            if (user is null || user.IsDeleted)
            {

                base.Results.Add(new ValidationResult($"Request failed. Kindly contact technical support."));
                return;
            }
            var business = (Business)viewModel;

            var email1Exists = _businessRepository.GetExistingBusinessInfo<BusinessResponseViewModel, string>("Email1", viewModel.Email1);


            if (email1Exists != null)
            {
                base.Results.Add(new ValidationResult($"Business Email is associated with another account, Kindly reconfirm details provided."));
                return;
            }

            var phoneNumberExists = _businessRepository.GetExistingBusinessInfo<BusinessResponseViewModel, string>("PhoneNumber1", viewModel.PhoneNumber1);


            if (phoneNumberExists != null)
            {
                base.Results.Add(new ValidationResult($"Business PhoneNumber is associated with another account, Kindly reconfirm details provided."));
                return;
            }

            business.UserAccountId = user.Id;
            business.CreatedBy = WebHelpers.CurrentUser.Email;
            await this.AddAsync(business);
        }

        public async Task<BusinessResponseViewModel?> GetBusinessDetail(Guid id)
        {
            var userId= Guid.Parse(WebHelpers.CurrentUser.UserId);
           
            var response = await _businessRepository.GetBusinessByIdAsync<BusinessResponseViewModel>(id, userId);

            if (response is null)
            {
                base.Results.Add(new ValidationResult($"Business not found. Kindly contact technical support."));
                return null;
            }

            return response;
        }

        public async Task<IEnumerable<BusinessResponseViewModel?>> GetUserBusinesses()
        {
            var userId = Guid.Parse(WebHelpers.CurrentUser.UserId);
            var result = await _businessRepository.GetUserBusinessesAsync<BusinessResponseViewModel>(userId);

            return result;
        }

        public async Task UpdateBusiness(EditBusinessViewModel viewModel)
        {
            if (!viewModel.Email1.IsValidEmail() || !viewModel.Email2.IsValidEmail())
            {
                base.Results.Add(new ValidationResult($"Email format is invalid."));
                return;
            }

            if (!viewModel.PhoneNumber1.IsValidPhoneNumber() || !viewModel.PhoneNumber2.IsValidPhoneNumber())
            {
                base.Results.Add(new ValidationResult($"Phone number format is invalid."));
                return;
            }

            
            var userId = Guid.Parse(WebHelpers.CurrentUser.UserId);

            var business = await _businessRepository.GetBusinessByIdAsync<BusinessDto>(viewModel.Id, userId);

            if (business is null)
            {
                base.Results.Add(new ValidationResult($"Business not found, Kindly reconfirm details provided."));
                return;
            }

            var email1Exists = _businessRepository.GetExistingBusinessInfo<BusinessResponseViewModel, string>("Email1", viewModel.Email1);


            if (email1Exists != null && business.Email1 != viewModel.Email1)
            {
                base.Results.Add(new ValidationResult($"Business Email is associated with another account, Kindly reconfirm details provided."));
                return;
            }

            var phoneNumberExists = _businessRepository.GetExistingBusinessInfo<BusinessResponseViewModel, string>("PhoneNumber1", viewModel.PhoneNumber1);


            if (phoneNumberExists != null && business.PhoneNumber1 != viewModel.PhoneNumber1)
            {
                base.Results.Add(new ValidationResult($"Business PhoneNumber is associated with another account, Kindly reconfirm details provided."));
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
