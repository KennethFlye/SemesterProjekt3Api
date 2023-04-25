namespace SemesterProjekt3Api.Security
{
    public class ApiAccount
    {
        public string JwtToken { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string GrantType { get; set; }
    }
}
