using AutoMapper;
using ProjectApp.Domain;
using SimpleAPITask.DTOs;

namespace SimpleAPITask.Profiles
{
    public class CommentProfile : AutoMapper.Profile
    {
        public CommentProfile()
        {
            CreateMap<Comment, CommentInputDTO>();
            CreateMap<Comment, CommentOutputDTO>();
            CreateMap<CommentInputDTO, Comment>();
            CreateMap<CommentOutputDTO, Comment>();
        }
    }
}
