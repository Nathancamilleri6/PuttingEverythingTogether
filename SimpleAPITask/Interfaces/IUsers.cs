using ProjectApp.Domain;
using SimpleAPITask.DTOs;
using System.Collections.Generic;

namespace SimpleAPITask.Interfaces
{
    public interface IUsers
    {
        public List<UserOutputDTO> GetUserDetails();
        public UserOutputDTO GetUserDetails(int id);
        public void AddUser(UserInputDTO user);
        public User GetUser(UserInputDTO user);
    }
}
