using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Teram.Web.Core;

namespace Teram.Module.Authentication.Tools
{

    public class MultilanguageIdentityErrorDescriber : IdentityErrorDescriber
    {
        private IStringLocalizer<SharedResource> localizer { get; }

        public MultilanguageIdentityErrorDescriber(IStringLocalizer<SharedResource> localizer)
        {
            this.localizer = localizer;
        }
        public override IdentityError DefaultError()
        {
            return new IdentityError { Code = nameof(DefaultError), Description = localizer["Unknown error"] };
        }
        public override IdentityError ConcurrencyFailure()
        {
            return new IdentityError { Code = nameof(ConcurrencyFailure), Description = localizer["ConcurrencyFailure"] };
        }
        public override IdentityError PasswordMismatch() { return new IdentityError { Code = nameof(PasswordMismatch), Description = localizer["PasswordMismatch"] }; }
        public override IdentityError InvalidToken()
        {
            return new IdentityError { Code = nameof(InvalidToken), Description = localizer["InvalidToken"] };
        }
        public override IdentityError LoginAlreadyAssociated()
        {
            return new IdentityError { Code = nameof(LoginAlreadyAssociated), Description = localizer["LoginAlreadyAssociated"] };
        }
        public override IdentityError InvalidUserName(string userName)
        {
            return new IdentityError { Code = nameof(InvalidUserName), Description =string.Format( localizer["InvalidUserName"],userName) };
        }
        public override IdentityError InvalidEmail(string email)
        {
            return new IdentityError { Code = nameof(InvalidEmail), Description = string.Format(localizer["InvalidEmail"],email) };
        }
        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError { Code = nameof(DuplicateUserName), Description = string.Format(localizer["DuplicateUserName"],userName) };
        }
        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError { Code = nameof(DuplicateEmail), Description = string.Format(localizer["DuplicateEmail"] ,email)};
        }
        public override IdentityError InvalidRoleName(string role)
        {
            return new IdentityError { Code = nameof(InvalidRoleName), Description = string.Format(localizer["InvalidRoleName"],role) };
        }

        public override IdentityError DuplicateRoleName(string role)
        {
            return new IdentityError { Code = nameof(DuplicateRoleName), Description = string.Format(localizer["DuplicateRoleName"] ,role)};
        }
        public override IdentityError UserAlreadyHasPassword()
        {
            return new IdentityError { Code = nameof(UserAlreadyHasPassword), Description = localizer["UserAlreadyHasPassword"] };
        }
        public override IdentityError UserLockoutNotEnabled()
        {
            return new IdentityError { Code = nameof(UserLockoutNotEnabled), Description = localizer["UserLockoutNotEnabled"] };
        }
        public override IdentityError UserAlreadyInRole(string role)
        {
            return new IdentityError { Code = nameof(UserAlreadyInRole), Description = string.Format(localizer["UserAlreadyInRole"] ,role)};
        }
        public override IdentityError UserNotInRole(string role)
        {
            return new IdentityError { Code = nameof(UserNotInRole), Description = string.Format(localizer["UserNotInRole"] ,role)};
        }
        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError { Code = nameof(PasswordTooShort), Description = string.Format(localizer["PasswordTooShort"],length) };
        }
        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError { Code = nameof(PasswordRequiresNonAlphanumeric), Description = localizer["PasswordRequiresNonAlphanumeric"] };
        }
        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError { Code = nameof(PasswordRequiresDigit), Description = localizer["PasswordRequiresDigit"] };
        }
        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError { Code = nameof(PasswordRequiresLower), Description = localizer["PasswordRequiresLower"] };
        }
        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError { Code = nameof(PasswordRequiresUpper), Description = localizer["PasswordRequiresUpper"] };
        }
        public override IdentityError RecoveryCodeRedemptionFailed()
        {
            return new IdentityError { Code = nameof(RecoveryCodeRedemptionFailed), Description = localizer["RecoveryCodeRedemptionFailed"] };
        }
        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
        {
            return new IdentityError { Code = nameof(PasswordRequiresUniqueChars), Description = string.Format(localizer["PasswordRequiresUniqueChars"],uniqueChars) };
        }


    }
}
