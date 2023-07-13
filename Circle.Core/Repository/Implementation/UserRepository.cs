using Circle.Core.Repository.Abstraction;
using Circle.Shared.Dapper;
using Circle.Shared.Dapper.Interfaces;
using Circle.Shared.Models.UserIdentity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Core.Repository.Implementation
{
    public class UserRepository : Service<AppUsers>, IUserRepository
    {
        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
