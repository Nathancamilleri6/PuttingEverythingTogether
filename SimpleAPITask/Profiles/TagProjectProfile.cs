using AutoMapper;
using ProjectApp.Domain;
using SimpleAPITask.DTOs;

namespace SimpleAPITask.Profiles
{
    public class TagProjectProfile : AutoMapper.Profile
    {
        public TagProjectProfile()
        {
            CreateMap<Project_Tag, TagInputDTO>();
            CreateMap<TagInputDTO, Project_Tag>();
            CreateMap<Project_Tag, TagProjectDTO>();
            CreateMap<TagProjectDTO, Project_Tag>();
        }
    }
}
