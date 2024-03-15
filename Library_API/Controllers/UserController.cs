using Library_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Library_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly LibraryContext _context;
        private readonly JWTSetting _jwtSetting;
        private readonly IRefreshTokenGenerator _tokenGenerator;

        public UserController(LibraryContext learnDb, IOptions<JWTSetting> options, IRefreshTokenGenerator refreshTokenGenerator)
        {
            _context = learnDb;
            _jwtSetting = options.Value;
            _tokenGenerator = refreshTokenGenerator;
        }

        [NonAction]
        public TokenResponse Authenticate(string username, Claim[] claims)
        {
            TokenResponse tokenResponse = new TokenResponse();
            var tokenkey = Encoding.UTF8.GetBytes(_jwtSetting.SecurityKey);
            var tokenhandler = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256)
            );
            tokenResponse.JWTToken = new JwtSecurityTokenHandler().WriteToken(tokenhandler);
            tokenResponse.RefreshToken = _tokenGenerator.GenerateToken(username);

            return tokenResponse;
        }



        [Route("Authenticate")]
        [HttpPost]
        public IActionResult Authenticate([FromBody] usercred user)
        {
            TokenResponse tokenResponse = new TokenResponse();

            var _user = _context.NguoiDungs.FirstOrDefault(e => e.NdUsername == user.username && e.NdPassword == user.password);
            if (_user == null)
            {
                return Unauthorized();
            }

            var claims = new Claim[]
            {
        new Claim(ClaimTypes.Name, _user.NdUsername),
        new Claim(ClaimTypes.Role, _user.QId),
        new Claim(ClaimTypes.NameIdentifier, _user.NdId.ToString()) // Add Nd_Id claim here
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_jwtSetting.SecurityKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(20),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            string finalToken = tokenHandler.WriteToken(token);

            tokenResponse.JWTToken = finalToken;
            tokenResponse.RefreshToken = _tokenGenerator.GenerateToken(user.username);

            return Ok(tokenResponse);
        }


        [HttpPost("Refresh")]
        public IActionResult Refresh([FromBody] TokenResponse token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;

            var principal = tokenHandler.ValidateToken(token.JWTToken, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.SecurityKey)),
                ValidateIssuer = false,
                ValidateAudience = false,
            }, out securityToken);

            var jwtToken = securityToken as JwtSecurityToken;
            if (jwtToken != null && !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.OrdinalIgnoreCase))
            {
                return Unauthorized();
            }

            var username = principal.Identity?.Name;
            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized();
            }

            var refreshTokenEntity = _context.Refreshtokens.FirstOrDefault(e => e.UserId == username && e.RefreshToken1 == token.RefreshToken);
            if (refreshTokenEntity == null)
            {
                return Unauthorized();
            }

            var result = Authenticate(username, principal.Claims.ToArray());
            if (result != null)
            {
                // You may want to return something based on the result
                return Ok(result);
            }

            return Unauthorized();
        }

    }
}