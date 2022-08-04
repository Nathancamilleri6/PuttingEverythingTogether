using AutoMapper;
using ProjectApp.Domain;
using SimpleAPITask.DTOs;

namespace SimpleAPITask.Profiles
{
    public class TagProfile : AutoMapper.Profile
    {
        public TagProfile()
        {
            CreateMap<Tag, TagInputDTO>();
            CreateMap<TagInputDTO, Tag>();
            CreateMap<Tag, Project_Tag>();
            CreateMap<Project_Tag, Tag>();
        }
    }
}
