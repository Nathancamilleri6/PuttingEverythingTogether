using ProjectApp.Domain;
using SimpleAPITask.DTOs;
using System.Collections.Generic;

namespace SimpleAPITask.Interfaces
{
    public interface IComments
    {
        public List<Comment> GetProjectComments(int id);
        public void AddComment(CommentInputDTO Comment);
        public Comment DeleteComment(int id);
    }
}
