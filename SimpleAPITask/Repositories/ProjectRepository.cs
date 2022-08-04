using AutoMapper;
using Microsoft.AspNetCore.Http;
using ProjectApp.Data;
using ProjectApp.Domain;
using SimpleAPITask.DTOs;
using SimpleAPITask.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SimpleAPITask.Repositories
{
    public class ProjectRepository : IProjects
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        readonly ProjectContext _dbContext = new();

        public ProjectRepository(IMapper Mapper, ProjectContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = Mapper;
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public List<ProjectOutputDTO> GetProjects(int userId)
        {
            try
            {
               // var assignees = _dbContext.Assignees.Where(assignee => assignee.AssigneeId == userId);
               // var createdProjects = _dbContext.Projects.Where(project => project.CreatorId == userId);

                var projects = _dbContext.Projects.Where(project => project.CreatorId == userId ||
                                    _dbContext.Assignees.Any(assignee => assignee.AssigneeId == userId && assignee.ProjectId == project.Id));
                return _mapper.Map<List<ProjectOutputDTO>>(projects);
            }
            catch
            {
                throw;
            }
        }

        public void EditProject(int id, ProjectOutputDTO Project)
        {
            try
            {
                var project = _dbContext.Projects.Find(id);
                project.Name = Project.Name;
                _dbContext.SaveChanges();
            }
            catch 
            {
                throw;
            }
        }

        public void AddProject(ProjectInputDTO project)
        {
            try
            {
                var email = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Email").Value;
                var user = _dbContext.Users.FirstOrDefault(x => x.Email == email);
                project.CreatorId = user.Id;
                _dbContext.Projects.Add(_mapper.Map<Project>(project));
                _dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public bool CheckProject(int id)
        {
            return _dbContext.Projects.Any(p => p.Id == id);
        }
    }
}
