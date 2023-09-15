using Circle.Core.ViewModels.User;
using Circle.Shared.Dapper;
using Circle.Shared.Models.UserIdentity;
using Circle.Shared.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Core.Repository.Abstraction
{
    public interface IUserProfileRepository : IService<UserProfile>
    {
        Task<T?> GetUserProfile<T>(Guid userId);
        Task<T> SetProfile<T>(SetUserProfileViewModel viewModel, Guid userId);
    }
}
