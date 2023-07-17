using Circle.Shared.Enums;
using Circle.Shared.Models.UserIdentity;
using Microsoft.AspNetCore.Http;
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

    public class EditUserViewModel
    {
        public string Username { get; set; }
        public string FirstName { get; set;}
        public string LastName { get; set;}
        public string MiddleName { get; set; }
        public Gender Gender { get; set; }
        public string PhoneNumber { get; set; }

    }

    public class UserDetailsViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public int Gender { get; set; }
        public DateTime CreatedOn { get; set; } 
        public string Location { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string BannerPictureUrl { get; set; }
        public string Bio { get; set; }

    }

    public class SetUserProfileViewModel
    {
        public string ProfileName { get; set; }
        public IFormFile ProfilePicture { get; set; }
        public IFormFile BannerPicture { get; set; }
        public string Location { get; set; }
        public string Bio { get; set; }
    }
}
