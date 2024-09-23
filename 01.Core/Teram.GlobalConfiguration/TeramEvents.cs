namespace Teram.GlobalConfiguration
{
    public class TeramEvents
    {
        #region Security
        public const int RegisterNewUser = 9000;
        public const int Loging = 9001;
        public const int UserLogOut = 9999;
        public const int InvalidLoggingAttempt= 9002;
        public const int TwoFactorRequired= 9003;
        public const int Lockout= 9004;
        public const int SendConfirmationEmail = 9005;
        public const int FailedToSendEmailConfirmation = 9006;
        public const int FailedToRegisterNewUser = 9007;
        public const int AccessDenied = 9008;
        public const int UnableToLoadUser = 9009;
        public const int EmailConfirmation= 9010;
        public const int EmailConfirmationFailed = 9011;
        public const int ChangingEmailFailed = 9013;
        #endregion
    }
}
