using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.Protocols;
using System.Globalization;
using Teram.IT.Module.ActiveDirectory.Models;

namespace Teram.IT.Module.ActiveDirectory.Services
{
    public static partial class ActiveDirectory
    {

        private const string Domain = "TERAMCHAP";
        public static LdapUserModel Authenticate(string username, string password)
        {
            try
            {
                using (var context = new PrincipalContext(ContextType.Domain, Domain))
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                    using (UserPrincipal userPrincipal = new UserPrincipal(context))
                    {
                        using (PrincipalSearcher searcher = new PrincipalSearcher(userPrincipal))
                        {

                            var allUsers = searcher.FindAll();

                            foreach (UserPrincipal currentuser in allUsers.Cast<UserPrincipal>())
                            {
                                // Access user properties
                                var x = currentuser;
                            }
                        }
                    }
                    var user = UserPrincipal.FindByIdentity(context, username);
                    if (user == null)
                    {
                        return new LdapUserModel { IsAuthorized = false, Message = "نام کاربری یا کلمه عبور اشتباه است لطفاً نام کاربری و کلمه عبور را بررسی نمایید." };
                    }
                    if (user.AccountExpirationDate != null)
                    {
                        if (DateTime.Compare(user.AccountExpirationDate.Value, DateTime.Now) <= 0)
                        {

                            return new LdapUserModel { IsAuthorized = false, Message = "نام کاربری شما منقضی شده است لطفاً با واحد زیر ساخت تماس حاصل فرمایید." };

                        }
                    }
                    var entry = (System.DirectoryServices.DirectoryEntry)user.GetUnderlyingObject();
                    var native = (ActiveDs.IADsUser)entry.NativeObject;

                    if (user.IsAccountLockedOut())
                    {
                        return new LdapUserModel { IsAuthorized = false, Message = "در حال حاضر نام کاربری شما قفل شده است چندین دقیقه دیگر مجددا تلاش کنید." };

                    }

                    if (!user.PasswordNeverExpires)
                    {
                        if (DateTime.Compare(native.PasswordExpirationDate, DateTime.Now) <= 0)
                        {
                            return new LdapUserModel { IsAuthorized = false, Message = "کلمه عبور شما منقضی شده است لطفاً با واحد زیر ساخت تماس حاصل فرمایید." };
                        }
                    }
                    var isValid = context.ValidateCredentials(username, password);
                    if (isValid == false)
                    {
                        return new LdapUserModel { IsAuthorized = false, Message = "نام کاربری یا کلمه عبور اشتباه است لطفاً نام کاربری و کلمه عبور را بررسی نمایید." };
                    }

                    return new LdapUserModel
                    {
                        IsAuthorized = true,
                        Name = user.Name,
                        Surname = user.Surname,
                        Email = user.EmailAddress
                    };
                }
            }
            catch (Exception ex)
            {
                if (ex is PrincipalServerDownException)
                {
                    return new LdapUserModel { IsAuthorized = false, Message = "احراز هویت به دلیل عدم اتصال به سرور دامین ترام چاپ امکان پذیر نیست، لطفاً با واحد زیرساخت تماس بگیرید." };
                }
                if (ex is NoMatchingPrincipalException)
                {
                    return new LdapUserModel { IsAuthorized = false, Message = "احراز هویت به دلیل عدم پیدا کردن سرور دامین ترام چاپ امکان پذیر نیست، لطفاً با واحد زیرساخت تماس بگیرید." };
                }

                if (ex is LdapException)
                {
                    var e = ((LdapException)ex);
                    return new LdapUserModel { IsAuthorized = false, Message = "احراز هویت به دلیل مشکل در سرور دامین ترام چاپ امکان پذیر نیست، لطفاً با واحد زیرساخت تماس بگیرید." };
                }
                return new LdapUserModel { IsAuthorized = false, Message = ex.InnerException != null ? ex.InnerException.Message : ex.Message };
            }
        }
    }
}
