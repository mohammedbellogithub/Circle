using Circle.Core.ViewModels.User;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Core.Validations
{
    public class UserRegisterationViewModelValidator : AbstractValidator<UserRegisterationViewModel>
    {
        public UserRegisterationViewModelValidator()
        {
            RuleFor(users => users.FirstName).NotNull().NotEmpty();
            RuleFor(users => users.LastName).NotNull().NotEmpty();
            RuleFor(users => users.MiddleName).NotNull().NotEmpty();
            RuleFor(users => users.Email).EmailAddress();
            RuleFor(users => users.PhoneNumber)
                .NotNull()
                .NotEmpty()
                .Matches("^[0]{1}[0-9]{10}$")
                .WithMessage("InValid Phone number");
        }
    }
}
