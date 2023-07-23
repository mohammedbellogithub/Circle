using Circle.Core.Repository.Abstraction;
using Circle.Core.Repository.Implementation;
using Circle.Core.ViewModels.Businesses;
using Circle.Shared.Dapper;
using Circle.Shared.Dapper.Interfaces;
using Circle.Shared.Helpers;
using Circle.Shared.Models.Businesses;
using System.ComponentModel.DataAnnotations;

namespace Circle.Core.Services.Businesses
{
    public class BusinessCategoryService : Service<BusinessCategory>, IBusinessCategoryService
    {
        private readonly IBusinessRepository _businessRepository;
        private readonly IBusinessCategoryRepository _businessCategoryRepository;
        public BusinessCategoryService(IUnitOfWork unitOfWork, IBusinessRepository businessRepository, IBusinessCategoryRepository businessCategoryRepository) : base(unitOfWork)
        {
            _businessRepository = businessRepository;
            _businessCategoryRepository = businessCategoryRepository;
        }

        public async Task CreateBusinessCategory(CreateBusinessCategoryViewModel viewModel)
        {
            var validUserBusiness  = await _businessRepository.GetBusinessByIdAsync<Business>(viewModel.BusinessId);

            if (validUserBusiness is null)
            {
                base.Results.Add(new ValidationResult("Invalid Business Id."));
                return;
            }
            var businessCategory = (BusinessCategory)viewModel;

            await this.AddAsync(businessCategory);
        }

        public async Task DeleteBusinessCategory(Guid id)
        {

            var businessCategories = await _businessCategoryRepository.GetUserBusinessCategories<BusinessCategory>(null);

            if (businessCategories is null)
            {
                base.Results.Add(new ValidationResult("Request failed. Kindly contact technical support."));
                return;
            }

            var businessCategory = businessCategories.Where(bc => bc != null && bc.Id == id)
                                                     .Select(bc => bc)
                                                     .FirstOrDefault();

            if (businessCategory is null)
            {
                base.Results.Add(new ValidationResult("Request failed. Kindly contact technical support."));
                return;
            }

            businessCategory.IsDeleted = true;
            businessCategory.DeletedBy = WebHelpers.CurrentUser.Email;
            businessCategory.DeletedOn = DateTime.Now;

            await this.UpdateAsync(businessCategory);
        }

        public async Task EditBusinessCategory(UpdateBusinessCategoryViewModel viewModel)
        {
            var businessCategories = await _businessCategoryRepository.GetUserBusinessCategories<BusinessCategory>(null);

            if (businessCategories is null)
            {
                base.Results.Add(new ValidationResult("Request failed. Kindly contact technical support."));
                return;
            }

            var businessCategory = businessCategories.Where(bc => bc != null && bc.Id == viewModel.Id)
                                                     .Select(bc => bc)
                                                     .FirstOrDefault();

            if (businessCategory is null)
            {
                base.Results.Add(new ValidationResult("Request failed. Kindly contact technical support."));
                return;
            }

            businessCategory.Name = viewModel.Name;
            businessCategory.Description = viewModel.Description;
            businessCategory.UniqueIdentiferCode = viewModel.UniqueIdentifierCode;

            await this.UpdateAsync(businessCategory);
        }

        public async Task<IEnumerable<BusinessCategoryViewModel?>> GetAllBusinessCategories(Guid BusinessId)
        {
            var businessCategories = await _businessCategoryRepository.GetUserBusinessCategories<BusinessCategoryViewModel>(BusinessId);


            return businessCategories;
        }
    }
}
