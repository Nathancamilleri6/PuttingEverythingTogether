using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectApp.Data;
using ProjectApp.Domain;
using SimpleAPITask.DTOs;
using SimpleAPITask.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace SimpleAPITask.Repositories
{
    public class UserRepository : IUsers
    {
        private readonly IMapper _mapper;
        readonly ProjectContext _dbContext = new();

        public UserRepository(IMapper Mapper, ProjectContext dbContext)
        {
            _mapper = Mapper;
            _dbContext = dbContext;
        }

        public List<UserOutputDTO> GetUserDetails()
        {
            try
            {
                return _mapper.Map<List<UserOutputDTO>>(_dbContext.Users); 
            }
            catch
            {
                throw;
            }
        }

        public void AddUser(UserInputDTO user)
        {
            try
            {
                User existingUser = _dbContext.Users.Where(u => u.Email == user.Email).FirstOrDefault();

                if(existingUser == null)
                {
                    user.Password = HashPassword(user.Password);
                    user.CreatedDate = DateTime.UtcNow;
                    _dbContext.Users.Add(_mapper.Map<User>(user));
                    _dbContext.SaveChanges();
                }
                else
                {
                    throw new Exception("User already exists!");
                }
            }
            catch
            {
                throw;
            }
        }

        public User GetUser(string email)
        {
            return _dbContext.Users.FirstOrDefault(u => u.Email == email);
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
