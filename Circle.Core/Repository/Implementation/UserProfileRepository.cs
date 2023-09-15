using Circle.Core.Repository.Abstraction;
using Circle.Core.ViewModels.User;
using Circle.Shared.Dapper;
using Circle.Shared.Dapper.Interfaces;
using Circle.Shared.Models.UserIdentity;
using Circle.Shared.Models.Users;
using Dapper;
using Microsoft.AspNetCore.Authentication.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Core.Repository.Implementation
{
    public class UserProfileRepository : Service<UserProfile>, IUserProfileRepository
    {
        public UserProfileRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<T?> GetUserProfile<T>(Guid userId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("Id", userId);
            var department = await this.ExecuteStoredProcedure<T>("[sp_get_user_profile]", parameters);

            return department.FirstOrDefault();
        }

        public async Task<UserProfileViewModel> SetProfile(SetUserProfileViewModel viewModel, Guid userId)
        {
            var user = await this.GetUserProfile<UserProfile>(userId);

            if (user is null)
            {
                return null;
            }
            user.ProfileName = viewModel.ProfileName;
            user.Location = viewModel.Location;
            user.Bio = viewModel.Bio;
            //call blob method that returns a string (Url) and append to particular model property


            await this.UpdateAsync(user);
            return true;
        }
    }
}
