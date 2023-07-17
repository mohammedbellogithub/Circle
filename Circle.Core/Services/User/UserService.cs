using Circle.Core.Dtos.User;
using Circle.Core.Repository.Abstraction;
using Circle.Core.Services.Cache;
using Circle.Core.ViewModels.User;
using Circle.Shared.Dapper;
using Circle.Shared.Dapper.Interfaces;
using Circle.Shared.Extensions;
using Circle.Shared.Helpers;
using Circle.Shared.Models.UserIdentity;
using Circle.Shared.Models.Users;
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
        private readonly IUserProfileRepository _userProfileRepository;


        public UserService(
            UserManager<AppUsers> userManager,
            RoleManager<AppRoles> roleManager,
            IUnitOfWork iuow,
            ICacheService cacheService,
            IUserProfileRepository userProfileRepository) : base(iuow)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _cacheService = cacheService;
            _userProfileRepository = userProfileRepository;
        }

        public async Task<UserResponseViewModel?> SignUpAsync(UserRegisterationViewModel viewModel)
        {
            //check if model is valid
            if (!base.IsValid(viewModel))
                return null;

            if (!viewModel.PhoneNumber.IsValidPhoneNumber())
            {
                base.Results.Add(new ValidationResult($"Invalid phone number format"));
                return null;
            }

            if (!viewModel.Email.IsValidEmail())
            {
                base.Results.Add(new ValidationResult($"Invalid email address format"));
                return null;
            }

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

            //set userProfile
            var profile = new UserProfile();
            profile.UserAccountId = user.Id;

            await _userProfileRepository.AddAsync(profile);

            var token = RandomGenerator.GenerateRandomNumber(6);
            _cacheService.SetCacheInfo(user.Email, token, duration: 5);
            var response = (UserResponseViewModel)user;
            response.Token = token;
            return response;

        }

        public async Task<IEnumerable<UserViewModel>> GetUsersAsync(string? keyword = null, string? roleName = null, int? pageIndex = 1, int? pageSize = 10)
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

        public async Task<UserResponseViewModel?> AddUserAsync(UserRegisterationViewModel viewModel)
        {
            if (!viewModel.PhoneNumber.IsValidPhoneNumber())
            {
                base.Results.Add(new ValidationResult($"Invalid phone number format"));
                return null;
            }

            if (!viewModel.Email.IsValidEmail())
            {
                base.Results.Add(new ValidationResult($"Invalid email address format"));
                return null;
            }

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

            user.CreatedBy = WebHelpers.CurrentUser.UserName; 
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
            _cacheService.SetCacheInfo(user.Email, response.Token, duration: 5);

            return response;
        }

        public async Task<UserResponseViewModel?> ChangePasswordAsync(string oldPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(WebHelpers.CurrentUser.UserId);

            var correctPassword = await _userManager.CheckPasswordAsync(user, oldPassword);

            if (!correctPassword)
            {
                await _userManager.AccessFailedAsync(user);
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

        public async Task DeactivateUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null || user.IsDeleted)
            {
                base.Results.Add(new ValidationResult($"User account not found. Kindly contact technical support."));
                return;
            }

            var sysAdmin = await _userManager.IsInRoleAsync(user, RoleHelpers.SYS_ADMIN);
            if (sysAdmin)
            {
                base.Results.Add(new ValidationResult($"User Deactivation failed. Kindly contact technical support."));
                return;
            }
            user.Activated = false;
            await _userManager.UpdateAsync(user);
        }

        public async Task SelfDeactivateAsync()
        {
            var user = await _userManager.FindByIdAsync(WebHelpers.CurrentUser.UserId);

            if (user is null || user.IsDeleted)
            {
                base.Results.Add(new ValidationResult($"User account not found. Kindly contact technical support."));
                return;
            }

            var sysAdmin = await _userManager.IsInRoleAsync(user, RoleHelpers.SYS_ADMIN);
            if (sysAdmin)
            {
                base.Results.Add(new ValidationResult($"User Deactivation failed. Kindly contact technical support."));
                return;
            }
            user.Activated = false;
            await _userManager.UpdateAsync(user);
        }

        public async Task DeleteUserAccountAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            var sysAdmin = await _userManager.IsInRoleAsync(user, RoleHelpers.SYS_ADMIN);
            
            if(user is null || user.IsDeleted)
            {
                base.Results.Add(new ValidationResult($"User account not found. Kindly contact technical support."));
                return;
            }

            if (sysAdmin)
            {
                base.Results.Add(new ValidationResult($"Request failed. Kindly contact technical support."));
                return;
            }
            user.IsDeleted = true;
            user.Activated = false;
            await _userManager.UpdateAsync(user);
        }

        public async Task EditUserAccount(EditUserViewModel viewModel)
        {
            if (!viewModel.PhoneNumber.IsValidPhoneNumber())
            {
                base.Results.Add(new ValidationResult($"Invalid phone number format"));
                return;
            }
            var user = await _userManager.FindByIdAsync(WebHelpers.CurrentUser.UserId);

            if (user is null || user.IsDeleted)
            {
                base.Results.Add(new ValidationResult($"User account not found. Kindly contact technical support."));
                return;
            }
            
            //check if username is taken
            var userNameTaken = await _userManager.FindByNameAsync(viewModel.Username);

            if (userNameTaken != null && user.UserName != viewModel.Username)
            {
                base.Results.Add(new ValidationResult($"Username: {viewModel.Username} is already Taken, Kindly choose another"));
                return;
            }

            //check if phone number exists
            var phoneExists = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == viewModel.PhoneNumber);
            if (phoneExists != null && user.PhoneNumber != viewModel.PhoneNumber)
            {
                base.Results.Add(new ValidationResult($"There is an account registered with this Phone number: {viewModel.PhoneNumber}, Kindly reconfirm."));
                return;
            }

            user.PhoneNumber = viewModel.PhoneNumber;
            user.UserName = viewModel.Username;
            user.FirstName = viewModel.FirstName;
            user.LastName = viewModel.LastName;
            user.Gender = (int)viewModel.Gender;
            user.MiddleName = viewModel.MiddleName;
            user.ModifiedBy = user.Email;
            user.ModifiedOn = DateTime.Now;

            await _userManager.UpdateAsync(user);
        }

        public  Task<UserDto?> GetUserDetails(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<UserDetailsViewModel?> UserBioDetails()
        {
            var user = await _userManager.FindByIdAsync(WebHelpers.CurrentUser.UserId);

            if (user == null || user.IsDeleted)
            {
                base.Results.Add(new ValidationResult($"Request failed. Kindly contact technical support."));
                return null;
            }

            var userProfile = await _userProfileRepository.GetUserProfile<UserDetailsViewModel>(user.Id);

            if (userProfile == null)
            {
                base.Results.Add(new ValidationResult("Kindly set up your Circle profile"));
                return null;
            }

            return userProfile;
        }

        public async Task SetProfile(SetUserProfileViewModel viewModel)
        {
            var user = await _userManager.FindByIdAsync(WebHelpers.CurrentUser.UserId);

            if (user == null || user.IsDeleted)
            {
                base.Results.Add(new ValidationResult($"Request failed. Kindly contact technical support."));
                return;
            }

            var Successfull = await _userProfileRepository.SetProfile(viewModel, user.Id);

            if (!Successfull)
            {
                base.Results.Add(new ValidationResult($"Request failed. Kindly contact technical support."));
                return;
            }
            //var userProfile = await _userRepository.GetUserProfile<UserDetailsViewModel>(user.Id);
        }
    }
}
