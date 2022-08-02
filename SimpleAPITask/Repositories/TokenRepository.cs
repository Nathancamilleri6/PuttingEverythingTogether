using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProjectApp.Domain;
using SimpleAPITask.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAPITask.Repositories
{
    public class TokenRepository : ITokens
    {
		private readonly IConfiguration iconfiguration;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public TokenRepository(IConfiguration iconfiguration, IHttpContextAccessor httpContextAccessor)
        {
            this.iconfiguration = iconfiguration;
            _httpContextAccessor = httpContextAccessor;
        }

		public async Task<Token> GenerateJWTTokensAsync(string email)
		{
			try
			{
				var tokenHandler = new JwtSecurityTokenHandler();
				var tokenKey = Encoding.UTF8.GetBytes(iconfiguration["JWT:Key"]);
				var tokenDescriptor = new SecurityTokenDescriptor
				{
					Subject = new ClaimsIdentity(new Claim[]
				  {
						new Claim("Email", email),
				  }),
					Expires = DateTime.Now.AddMinutes(1),
					SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
				};
				var token = tokenHandler.CreateToken(tokenDescriptor);
				var refreshToken = GenerateRefreshToken();

				// Cookie
				var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
				identity.AddClaim(new Claim("Email", email));
				var principal = new ClaimsPrincipal(identity);
				await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,	
					new AuthenticationProperties
					{
						IsPersistent = true,
						AllowRefresh = true,
						ExpiresUtc = DateTime.UtcNow.AddDays(1)
					});

				return new Token { AccessToken = tokenHandler.WriteToken(token), RefreshToken = refreshToken };
			}
			catch (Exception)
            {
				return null;
			}
		}

		public string GenerateRefreshToken()
		{
			var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

		public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
		{
			var Key = Encoding.UTF8.GetBytes(iconfiguration["JWT:Key"]);

			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuer = false,
				ValidateAudience = false,
				ValidateLifetime = false,
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Key),
				ClockSkew = TimeSpan.Zero
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || 
				!jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
		}
	}
}
