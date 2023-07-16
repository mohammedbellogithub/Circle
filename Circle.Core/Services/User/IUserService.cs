using Circle.Core.Dtos.User;
using Circle.Core.ViewModels.User;
using Circle.Shared.Configs;
using Circle.Shared.Dapper;
using Circle.Shared.Models.UserIdentity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Core.Services.User
{
    public interface IUserService : IService<AppUsers>
    {
        Task DeleteUserAccountAsync(string userId);
        Task DeactivateUserAsync(string userId);
        Task SelfDeactivateAsync();
        Task<IEnumerable<UserViewModel>> GetUsersAsync(string? keyword = null, string? roleName = null, int? pageIndex = 1, int? pageSize = 10);
        Task<UserResponseViewModel?> SignUpAsync(UserRegisterationViewModel user);
        Task<UserResponseViewModel?> AddUserAsync(UserRegisterationViewModel user);
        Task<bool> ValidateOTPAsync(string email, string code);
        Task<UserResponseViewModel?> ResendOTPAsync(string username);
        Task<UserResponseViewModel?> ChangePasswordAsync(string oldPassword, string newPassword);
        Task ResetPasswordAsync();
       
    }
}
