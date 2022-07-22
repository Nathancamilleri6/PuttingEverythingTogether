using Microsoft.AspNetCore.Mvc;
using SimpleAPITask.DTOs;
using SimpleAPITask.Models;
using System.Collections.Generic;

namespace SimpleAPITask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : ControllerBase
    {
        public static List<Project> projects = new List<Project>
        {
            new Project(1, "Project 1", new List<string> { "Comment 1" }),
            new Project(2, "Project 2", new List<string> { "Comment 2" }),
            new Project(3, "Project 3", new List<string> { "Comment 3" }),
        };

        [HttpGet]
        public IEnumerable<Project> RetrieveProjects()
        {
            return projects;
        }

        [HttpGet("{id}")]
        public Project RetrieveProject(int Id)
        {
            Project project = projects.Find(p => p.Id == Id);
            return project;
        }

        [HttpPost]
        public List<Project> AddProject([FromBody] ProjectDTO project)
        {
            projects.Add(new Project(project.Id, project.Name, new List<string> { project.Comment }));
            return projects;
        }

        [HttpPut("{id}")]
        public List<Project> AddCommentToProject(int Id, [FromBody] ProjectDTO project)
        {
            Project projectToEdit = projects.Find(p => p.Id == Id);
            int index = projects.IndexOf(projectToEdit);

            projects[index].Comments.Add(project.Comment);

            return projects;
        }
    }
}
