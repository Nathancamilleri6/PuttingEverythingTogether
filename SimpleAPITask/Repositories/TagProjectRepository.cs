using AutoMapper;
using ProjectApp.Data;
using ProjectApp.Domain;
using SimpleAPITask.DTOs;
using SimpleAPITask.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SimpleAPITask.Repositories
{
    public class TagProjectRepository : ITagProjects
    {
        private readonly IMapper _mapper;
        readonly ProjectContext _dbContext = new();

        public TagProjectRepository(IMapper Mapper, ProjectContext dbContext)
        {
            _mapper = Mapper;
            _dbContext = dbContext;
        }

        public void AddTagToProject(TagProjectDTO tag)
        {
            try
            {
                _dbContext.ProjectTags.Add(_mapper.Map<Project_Tag>(tag));
                _dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public List<Tag> GetProjectTags(int projectId)
        {
            List<Tag> projectTags = new();

            var tags = _mapper.Map<IList<TagProjectDTO>>(_dbContext.ProjectTags);

            foreach (var tag in tags)
            {
                if (tag.ProjectId == projectId)
                {
                    var toAdd = _dbContext.Tags.Where(t => t.Id == tag.TagId).FirstOrDefault();
                    projectTags.Add(toAdd);
                }
            }

            return projectTags;
        }


        public List<Tag> GetProjectNonTags(int projectId)
        {
            var tags = _dbContext.Tags.Where(tag =>
                            _dbContext.ProjectTags.All(p2 => p2.TagId != tag.Id || p2.ProjectId != projectId)).ToList();
            return _mapper.Map<List<Tag>>(tags);
        }
    }
}
