using eCommerceApp.Application.DTOs.Identity;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceApp.Application.Validations.Authentication
{
    public class CreateUserValidator : AbstractValidator<CreateUser>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.Fullname).NotEmpty()
                .WithMessage("Full name is required.");

            RuleFor(x => x.Email).NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");


             RuleFor(x => x.Password).NotEmpty()
                .WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be contain at least 8 characters long.")
                .Matches(@"[A-Z]").WithMessage("Password must be contain at least one uppercase latter.")
                .Matches(@"[a-z]").WithMessage("Password must be contain  at least one lowercase latter.")
                .Matches(@"\d").WithMessage("Password must be contain at least one number.")
                .Matches(@"[^\w]").WithMessage("Password must be contain at least one special character.");

             RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("Password do not match.");


        }
    }
}
