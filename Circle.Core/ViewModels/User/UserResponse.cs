using Circle.Shared.Enums;
using Circle.Shared.Models.UserIdentity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Core.ViewModels.User
{
    public class UserResponseViewModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public string Token { get; set; }


        //Mapping from Appuser to UserResponseViewModel
        public static explicit operator UserResponseViewModel(AppUsers source)
        {
            var destination = new UserResponseViewModel
            {
                FirstName = source.FirstName,
                LastName = source.LastName,
                MiddleName = source.MiddleName,
                Email = source.Email,
                UserName = source.UserName,
                PhoneNumber = source.PhoneNumber
            };
            return destination;
        }


    }


    public class UserRegisterationViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public string RoleId { get; set; }
        public string Password { get; set; }


        public static explicit operator AppUsers(UserRegisterationViewModel source)
        {
            var destination = new AppUsers
            {
                FirstName = source.FirstName,
                LastName = source.LastName,
                MiddleName = source.MiddleName,
                Email = source.Email,
                UserName = source.Username,
                PhoneNumber = source.PhoneNumber,
                Gender = (int)source.Gender
            };
            return destination;
        }

    }
}
