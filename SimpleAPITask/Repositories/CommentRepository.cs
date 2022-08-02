using AutoMapper;
using Microsoft.AspNetCore.Http;
using ProjectApp.Data;
using ProjectApp.Domain;
using SimpleAPITask.DTOs;
using SimpleAPITask.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleAPITask.Repositories
{
    public class CommentRepository : IComments
    {
        private readonly IMapper _mapper;
        readonly ProjectContext _dbContext = new();
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CommentRepository(IMapper Mapper, ProjectContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = Mapper;
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public List<Comment> GetProjectComments(int id)
        {
            var comments = _dbContext.Comments.Where(comment => comment.ProjectId == id).ToList();
            return _mapper.Map<List<Comment>>(comments);
        }

        public void AddComment(CommentInputDTO Comment)
        {
            try
            {
                var email = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Email").Value;
                var user = _dbContext.Users.FirstOrDefault(x => x.Email == email);
                Comment.CommentatorId = user.Id;
                Comment.DateTime = DateTime.UtcNow;
                _dbContext.Comments.Add(_mapper.Map<Comment>(Comment));
                _dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public Comment DeleteComment(int id)
        {
            try
            {
                var comment = _dbContext.Comments.Find(id);

                if(comment != null)
                {
                    _dbContext.Comments.Remove(comment);
                    _dbContext.SaveChanges();
                    return comment;
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
