using Circle.Core.Dtos.User;
using Circle.Shared.Enums;
using Circle.Shared.Models.UserIdentity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Core.ViewModels.User
{
    public class UserViewModel : BaseViewModel
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Role { get; set; }
        public bool IsActivated { get; set; }
        public bool IsDefaultPassword { get; set; }
        public string? Department { get; set; }
        public string? LastLoginDate { get; private set; }
        public Guid? RoleId { get; set; }
        public Gender Gender { get; set; }
        public string? Password { get; set; }
        public string? StaffNo { get; set; }

        public static explicit operator UserViewModel(UserDto source)
        {
            var destination = new UserViewModel();
            destination.Id = source.Id.ToString();
            destination.LastName = source.LastName;
            destination.FirstName = source.FirstName;
            destination.MiddleName =  source.MiddleName;
            destination.Username = source.Username;
            destination.Email = source.Email;
            destination.RoleId = source.RoleId;
            destination.Role = source.RoleName;
            destination.IsActivated = source.Activated;
            destination.PhoneNumber = source.PhoneNumber;
            destination.StaffNo = source.StaffNo;
            destination.TotalCount = source.TotalCount;
            destination.IsDefaultPassword = source.IsPasswordDefault;
            destination.Department = source.Department;
            destination.LastLoginDate = source.LastLoginDate.ToString();
            //destination.CreatedOn = source.CreatedOn
            //destination.ModifiedOn = source.ModifiedOn;
            return destination;
        }

        public static explicit operator AppUsers(UserViewModel source)
        {
            var destination = new AppUsers();
            destination.LastName = source.LastName;
            destination.FirstName = source.FirstName;
            destination.MiddleName = source.MiddleName;
            destination.UserName = source.Username;
            destination.Email = source.Email;
            destination.StaffNo = source.StaffNo;
            destination.Activated = source.IsActivated;
            destination.PhoneNumber = source.PhoneNumber;
            destination.Department = source.Department;
            return destination;
        }
    }


    public class UserProfileViewModel
    {

    }
}
