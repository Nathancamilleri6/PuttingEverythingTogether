using ProjectApp.Domain;
using SimpleAPITask.DTOs;
using System.Collections.Generic;

namespace SimpleAPITask.Interfaces
{
    public interface ITagProjects
    {
        public void AddTagToProject(TagProjectDTO tag);
        public List<Tag> GetProjectTags(int projectId);
        public List<Tag> GetProjectNonTags(int projectId);
    }
}
