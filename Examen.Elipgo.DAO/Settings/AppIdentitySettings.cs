using System;
using System.Collections.Generic;
using System.Text;

namespace Examen.Elipgo.DAO.Settings
{
    public class AppIdentitySettings
    {
        public LockoutSettings Lockout { get; set; }
        public PasswordSettings Password { get; set; }
    }
    public partial class LockoutSettings
    {
        public bool AllowedForNewUsers { get; set; }
        public int DefaultLockoutTimeSpanInMins { get; set; }
        public int MaxFailedAccessAttempts { get; set; }
    }

    public class PasswordSettings
    {
        public int RequiredLength { get; set; }
        public bool RequireLowercase { get; set; }
        public bool RequireUppercase { get; set; }
        public bool RequireDigit { get; set; }
        public bool RequireNonAlphanumeric { get; set; }
    }
}
