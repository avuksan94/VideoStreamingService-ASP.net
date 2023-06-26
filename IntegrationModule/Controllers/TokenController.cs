using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace IntegrationModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        //TOKEN TESTING
        private readonly IConfiguration _config;
        
        public TokenController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost]
        public ActionResult<Tokens> GetTokens()
        {
            // Get secret key bytes
            var tokenKey = Encoding.UTF8.GetBytes(_config.GetValue<string>("JWT:Key"));

            // Create a token descriptor (represents a token, kind of a "template" for token)
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _config.GetValue<string>("JWT:Issuer"),
                Audience = _config.GetValue<string>("JWT:Audience"),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            // Create token using that descriptor, serialize it and return it
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var serializedToken = tokenHandler.WriteToken(token);

            return new Tokens
            {
                Token = serializedToken
            };
        }

    }
}
