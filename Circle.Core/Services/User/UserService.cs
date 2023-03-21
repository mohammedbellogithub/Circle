using Circle.Core.Dtos.User;
using Circle.Shared.Dapper;
using Circle.Shared.Dapper.Interfaces;
using Circle.Shared.Models.UserIdentity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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

        public async Task<IEnumerable<UserDto>> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();

            var ListOfuserDto = new List<UserDto>();

            foreach (var user in users)
            {
                var userDto = new UserDto
                {
                    Id = user.Id.ToString(),
                    FirstName = user.FirstName,
                    MiddleName = user.MiddleName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                };

                ListOfuserDto.Add(userDto);
            }

            return ListOfuserDto;
        }

    }
}
