using Circle.Core.Dtos.User;
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
        Task<IEnumerable<UserDto>> GetUsers();
    }
}
