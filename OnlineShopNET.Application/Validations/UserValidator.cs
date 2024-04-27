using FluentValidation;
using OnlineShopNET.Domain.Config;
using OnlineShopNET.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace OnlineShopNET.Application.Validations
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(user => user.username).NotEmpty().MaximumLength(100);
            RuleFor(user => user.password).NotEmpty().MaximumLength(100);
            RuleFor(user => user.email).NotEmpty().EmailAddress();
            RuleFor(user => user.role_type).Must(roleType => roleType == 1 || roleType == 2)
            .WithMessage(Constant_Messages.INVALID_ROLE);
            RuleFor(user => user.email).Must(ValidEmail).WithMessage(Constant_Messages.INVALID_EMAIL);
        }

        private bool ValidEmail(string email)
        {
            // Basic email format validation
            if (!new EmailAddressAttribute().IsValid(email))
                return false;

            // Domain check
            try
            {
                string domain = email.Split('@')[1];
                IPHostEntry hostEntry = Dns.GetHostEntry(domain);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
