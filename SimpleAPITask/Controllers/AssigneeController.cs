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
    [Route("/api/assignees")]
    public class AssigneeController : ControllerBase
    {
        private readonly IAssignees _IAssignee;

        public AssigneeController(IAssignees IAssignee)
        {
            _IAssignee = IAssignee;
        }

        [HttpPost]
        public async Task<ActionResult<AssigneeInputDTO>> AddAssignee([FromBody] AssigneeInputDTO Assignee)
        {
            _IAssignee.AddAssignee(Assignee);
            return await Task.FromResult(Assignee);
        }

        // To improve (make like GetProjectNonAssignees)
        [HttpGet("{id}")]
        public async Task<ActionResult<List<UserOutputDTO>>> GetProjectAssignees(int id)
        {
            return await Task.FromResult(_IAssignee.GetProjectAssignees(id));
        }

        [Route("/api/assignees/{id}/inverse")]
        [HttpGet]
        public async Task<ActionResult<List<UserOutputDTO>>> GetProjectNonAssignees(int id)
        {
            return await Task.FromResult(_IAssignee.GetProjectNonAssignees(id));
        }
    }
}
