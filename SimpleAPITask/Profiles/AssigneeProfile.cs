using AutoMapper;
using ProjectApp.Domain;
using SimpleAPITask.DTOs;

namespace SimpleAPITask.Profiles
{
    public class AssigneeProfile : AutoMapper.Profile
    {
        public AssigneeProfile()
        {
            CreateMap<Project_Assignee, AssigneeInputDTO>();
            CreateMap<AssigneeInputDTO, Project_Assignee>();
        }
    }
}
