using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SemesterProjekt3Api.Security
{
    

 
    {
        public class SecurityHelper
    {
        private readonly IConfiguration _configuration;

        //fetches config from more sources
        public SecurityHelper(IConfiguration inConfiguration)
        {
            _configuration = inConfiguration;
        }

        //creates key for signing
        public SymmetricSecurityKey? GetSecurityKey()
        {
            SymmetricSecurityKey? SIGNING_KEY = null;
            if (_configuration != null)
            {
                string SECRET_KEY = _configuration["SECRET_KEY"];
                SIGNING_KEY = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET_KEY));
            }
            return SIGNING_KEY;
        }

        internal bool IsValidUsernameAndPassword(string username, string password)
        {
            string allowedUsername = _configuration["AllowDesktopApp:Username"]; 
            string allowedPassword = _configuration["AllowDesktopApp:Password"]; 
            bool credentialsOk = (username.Equals(allowedUsername)) && (password.Equals(allowedPassword)); 
            return credentialsOk;
        }
    }
}