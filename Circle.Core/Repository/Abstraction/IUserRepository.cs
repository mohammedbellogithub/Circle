using Circle.Shared.Dapper;
using Circle.Shared.Models.UserIdentity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Core.Repository.Abstraction
{
    public interface IUserRepository : IService<AppUsers>
    {

    }
}
