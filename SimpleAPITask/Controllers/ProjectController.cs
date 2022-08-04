using Microsoft.AspNetCore.Mvc;
using SimpleAPITask.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using SimpleAPITask.Interfaces;

namespace SimpleAPITask.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/projects")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjects _IProject;

        public ProjectController(IProjects IProject)
        {
            _IProject = IProject;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<ProjectOutputDTO>>> GetProjects(int id)
        {
            return await Task.FromResult(_IProject.GetProjects(id));
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<ProjectOutputDTO>> EditProject(int id, [FromBody] ProjectOutputDTO Project)
        {
            if (id != Project.Id)
            {
                return BadRequest();
            }

            try
            {
                _IProject.EditProject(id, Project);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return await Task.FromResult(Project);
        }

        [HttpPost]
        public async Task<ActionResult<ProjectInputDTO>> AddProject([FromBody] ProjectInputDTO project)
        {
            _IProject.AddProject(project);
            return await Task.FromResult(project);
        }

        private bool ProjectExists(int id)
        {
            return _IProject.CheckProject(id);
        }
    }
}
