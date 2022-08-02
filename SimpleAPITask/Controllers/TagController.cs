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
    [Route("/api/tags")]
    public class TagController : ControllerBase
    {
        private readonly ITags _ITag;

        public TagController(ITags ITag)
        {
            _ITag = ITag;
        }

        [HttpGet]
        public async Task<ActionResult<List<Tag>>> GetTags()
        {
            return await Task.FromResult(_ITag.GetTags());
        }

        [HttpPost]
        public async Task<ActionResult<TagInputDTO>> AddTag([FromBody] TagInputDTO Tag)
        {
            _ITag.AddTag(Tag);
            return await Task.FromResult(Tag);
        }
    }
}
