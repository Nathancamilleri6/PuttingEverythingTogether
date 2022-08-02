using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectApp.Domain;
using SimpleAPITask.DTOs;
using SimpleAPITask.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleAPITask.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/comments")]
    public class CommentController : ControllerBase
    {
        private readonly IComments _IComment;

        public CommentController(IComments IComment)
        {
            _IComment = IComment;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Comment>>> GetProjectComments(int id)
        {
            return await Task.FromResult(_IComment.GetProjectComments(id));
        }

        // FIX DATETIME ISSUE (COMING UP AS 0001-01-01T00:00:00)
        [HttpPost]
        public async Task<ActionResult<CommentInputDTO>> AddComment([FromBody] CommentInputDTO Comment)
        {
            _IComment.AddComment(Comment);
            return await Task.FromResult(Comment);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Comment>> DeleteComment(int id)
        {
            var comment = _IComment.DeleteComment(id);
            return await Task.FromResult(comment);
        }
    }
}
