using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SemesterProjekt3Api.Security;
using System.IdentityModel.Tokens.Jwt;

namespace SemesterProjekt3Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        // Fetches configuration from more sources
        public TokensController(IConfiguration inConfiguration)
        {
            _configuration = inConfiguration;
        }

        [Route("/token")]
        [HttpPost] // Generate and return a JWT token
        public IActionResult Create([FromForm] string username, string password, string grant_type)
        {
            bool hasInput = ((!String.IsNullOrWhiteSpace(username)) && (!String.IsNullOrWhiteSpace(password)));
            // Only return JWT token if credentials are valid
            SecurityHelper secUtil = new SecurityHelper(_configuration);
            if (hasInput && secUtil.IsValidUsernameAndPassword(username, password))
            {
                string jwtToken = GenerateToken(username, grant_type);
                return new ObjectResult(jwtToken);
            }
            else
            {
                return BadRequest();
            }
        }

        private string GenerateToken(string username, string grantType)
        {
            string tokenString;
            SecurityHelper secUtil = new SecurityHelper(_configuration);
            // Create header with algorithm and token type - and secret added
            SymmetricSecurityKey? SIGNING_KEY = secUtil.GetSecurityKey();
            SigningCredentials credentials = new SigningCredentials(SIGNING_KEY, SecurityAlgorithms.HmacSha256);
            JwtHeader header = new JwtHeader(credentials);

            // Time to live for newly created JWT Token
            int ttlInMinutes = 10;
            DateTime expiry = DateTime.UtcNow.AddMinutes(ttlInMinutes);
            int ts = (int)(expiry - new DateTime(1970, 1, 1)).TotalSeconds;
            var payload = new JwtPayload {
                { "sub", "testSubject" },
                { "Name", username },
                { "email", "smithtest@tesst.com" },
                { "granttype", grantType },
                { "exp", ts },
                { "iss", "https://localhost:7150" }, // Issuer - the party that generates the JWT
                { "aud", "https://localhost:7150" } // Audience - The address of the resource
            };
            JwtSecurityToken secToken = new JwtSecurityToken(header, payload);

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            tokenString = handler.WriteToken(secToken);

            return tokenString;
        }
    }
}