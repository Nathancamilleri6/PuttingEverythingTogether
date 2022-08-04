using ProjectApp.Domain;
using SimpleAPITask.DTOs;
using System.Collections.Generic;

namespace SimpleAPITask.Interfaces
{
    public interface IUsers
    {
        public List<UserOutputDTO> GetUserDetails();
        public void AddUser(UserInputDTO user);
        public User GetUser(string email);
    }
}
