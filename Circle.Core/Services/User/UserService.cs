using Circle.Core.Dtos.User;
using Circle.Core.ViewModels.User;
using Circle.Shared.Dapper;
using Circle.Shared.Dapper.Interfaces;
using Circle.Shared.Helpers;
using Circle.Shared.Models.UserIdentity;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Core.Services.User
{
    public class UserService : Service<AppUsers>, IUserService
    {
        private readonly SignInManager<AppUsers> _signInManager;
        private readonly IPasswordHasher<AppUsers> _passwordHasher;
        private readonly UserManager<AppUsers> _userManager;
        private readonly RoleManager<AppRoles> _roleManager;
        private readonly ILogger<UserService> _logger;
        //private readonly AppSettings _appSettings;
        //private readonly IUserRoleService _userRoleService;
        //private readonly INotifier _notifier;
        private readonly IConfiguration _configuration;


        public UserService(SignInManager<AppUsers> signInManager,
            IPasswordHasher<AppUsers> passwordHasher, 
            UserManager<AppUsers> userManager,
            RoleManager<AppRoles> roleManager, 
            ILogger<UserService> logger,
            IUnitOfWork iuow,
            IConfiguration configuration) : base(iuow)
        {
            _signInManager = signInManager;
            _passwordHasher = passwordHasher;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<UserResponseViewModel?> SignUp(UserRegisterationViewModel viewModel)
        {
            //check if model is valid
            if (!base.IsValid(viewModel))
                return default;

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
            user.EmailConfirmed = false;
            user.PhoneNumberConfirmed = true;
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

            //generate email comfirmation token
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

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
    }
}
