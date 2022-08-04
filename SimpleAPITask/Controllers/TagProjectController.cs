using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectApp.Domain;
using SimpleAPITask.DTOs;
using SimpleAPITask.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleAPITask.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/tagProjects")]
    public class TagProjectController : ControllerBase
    {
        private readonly ITagProjects _ITagProjects;

        public TagProjectController(ITagProjects ITagProject)
        {
            _ITagProjects = ITagProject;
        }

        [HttpPost]
        public async Task<ActionResult<TagProjectDTO>> AddTagToProject([FromBody] TagProjectDTO Tag)
        {
            _ITagProjects.AddTagToProject(Tag);
            return await Task.FromResult(Tag);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Tag>>> GetProjectTags(int id)
        {
            return await Task.FromResult(_ITagProjects.GetProjectTags(id));
        }

        [Route("/api/tagProjects/{id}/inverse")]
        [HttpGet]
        public async Task<ActionResult<List<Tag>>> GetProjectNonTags(int id)
        {
            return await Task.FromResult(_ITagProjects.GetProjectNonTags(id));
        }
    }
}
