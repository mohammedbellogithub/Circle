using Circle.Core.Dtos.User;
using Circle.Core.ViewModels.User;
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
        Task<IEnumerable<UserViewModel>> GetUsers(string? keyword = null, string? roleName = null, int? pageIndex = 1, int? pageSize = 10);

        Task<UserResponseViewModel?> SignUp(UserRegisterationViewModel user);

        Task<UserResponseViewModel?> AddUser(UserRegisterationViewModel user);
    }
}
