﻿using SimpleAPITask.DTOs;
using System.Collections.Generic;

namespace SimpleAPITask.Interfaces
{
    public interface IProjects
    {
        public List<ProjectOutputDTO> GetProjects(int userId);
        public void EditProject(int id, ProjectOutputDTO Project);
        public void AddProject(ProjectInputDTO Project);
        public bool CheckProject(int id);
    }
}
