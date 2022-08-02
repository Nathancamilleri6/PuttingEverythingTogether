using BCrypt.Net;
using ProjectApp.Data;
using ProjectApp.Domain;
using SimpleAPITask.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleAPITask.Repositories
{
    public class UserServiceRepository : IUserServices
    {
		readonly ProjectContext _dbContext = new();
		private readonly ITokens _IToken;

		public UserServiceRepository(ProjectContext dbContext, ITokens IToken)
		{
			_dbContext = dbContext;
			_IToken = IToken;
		}

		public UserRefreshToken AddUserRefreshToken(UserRefreshToken refreshToken)
		{
			_dbContext.UserRefreshTokens.Add(refreshToken);
			return refreshToken;
		}

		public void DeleteUserRefreshToken(string email, string refreshToken)
		{
			var item = _dbContext.UserRefreshTokens.FirstOrDefault(x => x.Email == email && x.RefreshToken == refreshToken);
			if (item != null)
			{
				_dbContext.UserRefreshTokens.Remove(item);
			}
		}

		public UserRefreshToken GetSavedRefreshToken(string email, string refreshToken)
		{
			return _dbContext.UserRefreshTokens.FirstOrDefault(x => x.Email == email && x.RefreshToken == refreshToken && x.IsActive == true);
		}

		public int SaveCommit()
		{
			return _dbContext.SaveChanges();
		}

		public async Task<Token> Authenticate(User user)
		{
			try
			{
				var validUser = IsValidUser(user);

				if (!validUser)
				{
					throw new UnauthorizedAccessException();
				}

				var token = await _IToken.GenerateJWTTokensAsync(user.Email);

				if (token == null)
				{
					throw new ArgumentNullException();
				}

				// saving refresh token to the db
				UserRefreshToken userRefreshToken = new()
				{
					RefreshToken = token.RefreshToken,
					Email = user.Email
				};

				AddUserRefreshToken(userRefreshToken);
				SaveCommit();
				return token;
			}
			catch
			{
				throw;
			}
		}

		public async Task<Token> Refresh(Token token)
		{
            try
            {
				var principal = _IToken.GetPrincipalFromExpiredToken(token.AccessToken);
				var email = principal.Claims.FirstOrDefault(claim => claim.Type == "Email").Value;
				//retrieve the saved refresh token from database
				var savedRefreshToken = GetSavedRefreshToken(email, token.RefreshToken);

				if (savedRefreshToken.RefreshToken != token.RefreshToken)
				{
					throw new UnauthorizedAccessException();
				}

				var newJwtToken = await _IToken.GenerateJWTTokensAsync(email);
				if (newJwtToken == null)
				{
					throw new UnauthorizedAccessException();
				}

				// saving refresh token to the db
				UserRefreshToken obj = new()
				{
					RefreshToken = newJwtToken.RefreshToken,
					Email = email
				};

				DeleteUserRefreshToken(email, token.RefreshToken);
				AddUserRefreshToken(obj);
				SaveCommit();
				return newJwtToken;
			}
            catch
            {
				throw;
            }
		}

		public bool IsValidUser(User User)
		{
			var u = _dbContext.Users.FirstOrDefault(o => o.Email == User.Email);
			if (u != null && CheckHash(User.Password, u.Password))
			{
				return true;
			}
			return false;
		}

		public bool CheckHash(string password, string hashed)
        {
			return BCrypt.Net.BCrypt.Verify(password, hashed);
		}
	}
}
