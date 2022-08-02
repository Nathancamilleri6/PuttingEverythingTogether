using AutoMapper;
using ProjectApp.Domain;
using SimpleAPITask.DTOs;

namespace SimpleAPITask.Profiles
{
    public class ProjectProfile : AutoMapper.Profile
    {
        public ProjectProfile()
        {
            CreateMap<Project, ProjectInputDTO>();
            CreateMap<Project, ProjectOutputDTO>();
            CreateMap<ProjectInputDTO, Project>();
            CreateMap<ProjectOutputDTO, Project>();
        }
    }
}
