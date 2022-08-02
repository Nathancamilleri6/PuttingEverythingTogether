using AutoMapper;
using ProjectApp.Domain;
using SimpleAPITask.DTOs;

namespace SimpleAPITask.Profiles
{
    public class UserProfile : AutoMapper.Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserInputDTO>();
            CreateMap<User, UserOutputDTO>();
            CreateMap<UserInputDTO, User>();
            CreateMap<UserOutputDTO, User>();
        }
    }
}
