using SimpleAPITask.DTOs;
using System.Collections.Generic;

namespace SimpleAPITask.Interfaces
{
    public interface IAssignees
    {
        public void AddAssignee(AssigneeInputDTO assignee);
        public List<UserOutputDTO> GetProjectAssignees(int id);
        public List<UserOutputDTO> GetProjectNonAssignees(int id);
    }
}
