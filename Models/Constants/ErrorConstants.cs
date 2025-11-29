using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Constants
{
    public class ErrorConstants
    {
        public const string InValid = "You have entered Invalid Id";
        public const string Exists = "User already exists";
        public const string RegistrationFailed = "Registration has failed";
        public const string NotFound = "UserNotFound";
        public const string RoleNotFound = "RoleNotFound";
        public const string LoginFailed = "Login has failed";
        public const string Failed = "Could not add the role";
        public const string Internal = "Internal Server Errror";
        public const string InvalidToken = "You have given Invalid refresh token";

    }
}
