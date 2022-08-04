using AutoMapper;
using ProjectApp.Data;
using ProjectApp.Domain;
using SimpleAPITask.DTOs;
using SimpleAPITask.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SimpleAPITask.Repositories
{
    public class AssigneeRepository : IAssignees
    {
        private readonly IMapper _mapper;
        readonly ProjectContext _dbContext = new();

        public AssigneeRepository(IMapper Mapper, ProjectContext dbContext)
        {
            _mapper = Mapper;
            _dbContext = dbContext;
        }

        public void AddAssignee(AssigneeInputDTO assignee)
        {
            try
            {
                _dbContext.Assignees.Add(_mapper.Map<Project_Assignee>(assignee));
                _dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public List<UserOutputDTO> GetProjectAssignees(int id)
        {
            List<UserOutputDTO> projectAssignees = new();
            var assignees = _mapper.Map<IList<AssigneeInputDTO>>(_dbContext.Assignees);

            foreach (AssigneeInputDTO assignee in assignees)
            {
                if (assignee.ProjectId == id)
                {
                    projectAssignees.Add(_mapper.Map<UserOutputDTO>(_dbContext.Users.Where(user => user.Id == assignee.AssigneeId).FirstOrDefault()));
                }
            }

            return projectAssignees;
        }

        public List<UserOutputDTO> GetProjectNonAssignees(int id)
        {
            var users = _dbContext.Users.Where(user => _dbContext.Assignees.All(p2 => p2.AssigneeId != user.Id || p2.ProjectId != id)).ToList();
            return _mapper.Map<List<UserOutputDTO>>(users);
        }
    }
}
