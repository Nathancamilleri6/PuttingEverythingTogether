using ProjectApp.Domain;
using System.Threading.Tasks;

namespace SimpleAPITask.Interfaces
{
    public interface IUserServices
    {
		public bool IsValidUser(User users);
		public UserRefreshToken AddUserRefreshToken(UserRefreshToken user);
		public UserRefreshToken GetSavedRefreshToken(string email, string refreshtoken);
		public void DeleteUserRefreshToken(string email, string refreshToken);
		public int SaveCommit();
		public Task<Token> Authenticate(User User);
		public Task<Token> Refresh(Token Token);
	}
}
