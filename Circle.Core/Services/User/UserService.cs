using Circle.Core.Dtos.User;
using Circle.Core.Services.Cache;
using Circle.Core.ViewModels.User;
using Circle.Shared.Dapper;
using Circle.Shared.Dapper.Interfaces;
using Circle.Shared.Helpers;
using Circle.Shared.Models.UserIdentity;
using Circle.Shared.Utils;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Circle.Core.Services.User
{
    public class UserService : Service<AppUsers>, IUserService
    {
        private readonly UserManager<AppUsers> _userManager;
        private readonly RoleManager<AppRoles> _roleManager;
        private readonly ICacheService _cacheService;


        public UserService(
            UserManager<AppUsers> userManager,
            RoleManager<AppRoles> roleManager,
            IUnitOfWork iuow,
            ICacheService cacheService) : base(iuow)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _cacheService = cacheService;
        }

        public async Task<UserResponseViewModel?> SignUp(UserRegisterationViewModel viewModel)
        {
            //check if model is valid
            if (!base.IsValid(viewModel))
                return null;

            //check if the user exists
            var userExists = _userManager.Users.FirstOrDefault(x => x.FirstName == viewModel.FirstName
                                                && x.LastName == viewModel.LastName
                                                && x.Email == viewModel.Email);
            if (userExists != null)
            {
                base.Results.Add(new ValidationResult($"{viewModel.LastName} {viewModel.FirstName} exists."));
                return default;
            }

            //check if Email is already registered
            var user = await _userManager.FindByEmailAsync(viewModel.Email);
            if (user != null)
            {
                base.Results.Add(new ValidationResult($"There is an account registered with this Email {viewModel.Email}, Kindly login."));
                return default;
            }

            //check if username is taken
            var userNameTaken = await _userManager.FindByNameAsync(viewModel.Username);
            if (userNameTaken != null)
            {
                base.Results.Add(new ValidationResult($"Username: {viewModel.Username} is already Taken, Kindly choose another"));
                return default;
            }

            //check if phone number exists
            var phoneExists = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == viewModel.PhoneNumber);
            if (phoneExists != null)
            {
                base.Results.Add(new ValidationResult($"There is an account registered with this Phone number: {viewModel.PhoneNumber}, Kindly reconfirm."));
                return default;
            }

            user = (AppUsers)viewModel;
            user.Activated = false;
            user.IsPasswordDefault = false;


            //password validation
            if (string.IsNullOrEmpty(viewModel.Password))
            {
                base.Results.Add(new ValidationResult($"Kindly enter a valid password."));
                return default;
            }

            var createResult = await _userManager.CreateAsync(user, viewModel.Password);

            if (!createResult.Succeeded)
            {
                base.Results.AddRange(createResult.Errors.Select(e => new ValidationResult(e.Description)));
                return default;

            }
            user.CreatedBy = "SIGN UP";
            user.ModifiedBy = "DEFAULT";
            user.UserType = 1;
            //Add user to default role
            createResult = await _userManager.AddToRoleAsync(user, RoleHelpers.DEFAULT);
            if (!createResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                base.Results.AddRange(createResult.Errors.Select(e => new ValidationResult(e.Description)));
                return default;
            }
            user.CreatedOn = DateTime.Now;

            var token = RandomGenerator.GenerateRandomNumber(6);
            _cacheService.SetCacheInfo(user.Email, token);
            var response = (UserResponseViewModel)user;
            response.Token = token;
            return response;

        }

        public async Task<IEnumerable<UserViewModel>> GetUsers(string? keyword = null, string? roleName = null, int? pageIndex = 1, int? pageSize = 10)
        {
            pageIndex = (pageIndex < 1 || !pageIndex.HasValue) ? 1 : pageIndex.Value;
            pageSize = (pageSize <= 1 || !pageSize.HasValue) ? 10 : pageSize.Value;



            var parameters = new DynamicParameters();
            parameters.Add("@Keyword", keyword);
            parameters.Add("@RoleName", roleName);
            parameters.Add("@PageIndex", pageIndex);
            parameters.Add("@PageSize", pageSize);

            var result = await this.ExecuteStoredProcedure<UserDto>("[sp_get_users]", parameters);
            return result.Select(r => (UserViewModel)r);
        }

        public async Task<UserResponseViewModel?> AddUser(UserRegisterationViewModel viewModel)
        {
            //check if the user exists
            var userExists = _userManager.Users.FirstOrDefault(x => x.FirstName == viewModel.FirstName
                                                && x.LastName == viewModel.LastName
                                                && x.Email == viewModel.Email);

            if (userExists != null)
            {
                base.Results.Add(new ValidationResult($"{viewModel.LastName} {viewModel.FirstName} exists."));
                return default;
            }

            //check if Email is already registered
            var userEmail = await _userManager.FindByEmailAsync(viewModel.Email);
            if (userEmail != null)
            {
                base.Results.Add(new ValidationResult($"There is an account registered with this Email {viewModel.Email}, Kindly login."));
                return default;
            }

            //check if username is taken
            var userNameTaken = await _userManager.FindByNameAsync(viewModel.Username);
            if (userNameTaken != null)
            {
                base.Results.Add(new ValidationResult($"Username: {viewModel.Username} already Taken, Kindly choose another"));
                return default;
            }

            //check if phone number exists
            var phoneExists = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == viewModel.PhoneNumber);
            if (phoneExists != null)
            {
                base.Results.Add(new ValidationResult($"There is an account registered with this Phone number: {viewModel.PhoneNumber}, Kindly reconfirm."));
                return default;
            }

           
            //check if the role is system admin
            if (RoleHelpers.SYS_ADMIN_ID().ToString() == viewModel.RoleId)
            {
                base.Results.Add(new ValidationResult($"Role is currently restricted."));
                return default;
            }

            var user = (AppUsers)viewModel;
            user.Activated = false;
            user.EmailConfirmed = false;
            user.PhoneNumberConfirmed = true;
            user.IsPasswordDefault = false;


            var createResult = await _userManager.CreateAsync(user);

            if (!createResult.Succeeded)
            {
                base.Results.AddRange(createResult.Errors.Select(e => new ValidationResult(e.Description)));
                return default;
            }

            //check if role exists
            bool RoleExits = false;
            var role = await _roleManager.FindByIdAsync(viewModel.RoleId);

            if (role != null)
            {
                //Add user to role
                await _userManager.AddToRoleAsync(user, role.Name);
                RoleExits = true;
            }

            if (!RoleExits)
            {
                base.Results.AddRange(createResult.Errors.Select(e => new ValidationResult(e.Description)));
                return default;
            }

            //user.CreatedBy = WebHelpers.CurrentUser.UserName; ---- set current enterprise user logged in
            //model.ModifiedBy = WebHelpers.CurrentUser.UserName;

            //generate email comfirmation token
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var response = (UserResponseViewModel)user;
            response.Token = token;
            return response;
        }

        public async Task<bool>  ValidateOTPAsync(string email, string code)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null || user.IsDeleted)
            {
                base.Results.Add(new ValidationResult("invalid user"));
                return false;
            }

            if (await _userManager.IsLockedOutAsync(user))
            {
                base.Results.Add(new ValidationResult("Your Cicle account is temorarily restricted. Kindly try again later."));
                return false;
            }

            var otp = _cacheService.GetCacheKey(user.Email);

            if (otp is not null && otp == code)
            {
                
                user.Activated = true;
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
                return true;
            }

            if (otp is  null || otp != code)
            {
                await _userManager.AccessFailedAsync(user);
                base.Results.Add(new ValidationResult($"OTP validation failed"));
                return false;
            }

            return false;
        }

        public async Task<UserResponseViewModel?> ResendOTPAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if(user is null || user.IsDeleted)
            {
                this.Results.Add(new ValidationResult("user not found"));
                return null;
            }

            var response = (UserResponseViewModel)user;
            
            response.Token = RandomGenerator.GenerateRandomNumber(6);
            _cacheService.SetCacheInfo(user.Email, response.Token);

            return response;
        }

        public async Task<UserResponseViewModel?> ChangePasswordAsync(string oldPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(WebHelpers.CurrentUser.UserId);

            var correctPassword = await _userManager.CheckPasswordAsync(user, oldPassword);

            if (!correctPassword)
            {
                ++user.AccessFailedCount;
                await _userManager.UpdateAsync(user);
                base.Results.Add(new ValidationResult($"Kindly ensure that your password is correct. Thank you "));
                return null;
            }

            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

            if (!result.Succeeded)
            {
                base.Results.Add(new ValidationResult($"Kindly try again later or contact support. Thank you "));
                return null;
            }

            var response = (UserResponseViewModel)user;
            return response;
        }

        public Task ResetPasswordAsync()
        {
            throw new NotImplementedException();
        }
    }
}
