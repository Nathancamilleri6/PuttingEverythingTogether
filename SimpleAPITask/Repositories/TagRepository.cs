using AutoMapper;
using ProjectApp.Data;
using ProjectApp.Domain;
using SimpleAPITask.DTOs;
using SimpleAPITask.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleAPITask.Repositories
{
    public class TagRepository : ITags
    {
        private readonly IMapper _mapper;
        readonly ProjectContext _dbContext = new();

        public TagRepository(IMapper Mapper, ProjectContext dbContext)
        {
            _mapper = Mapper;
            _dbContext = dbContext;
        }

        public void AddTag(TagInputDTO tag)
        {
            try
            {
                _dbContext.Tags.Add(_mapper.Map<Tag>(tag));
                _dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public List<Tag> GetTags()
        {
            try
            {
                return _mapper.Map<List<Tag>>(_dbContext.Tags);
            }
            catch
            {
                throw;
            }
        }
    }
}
