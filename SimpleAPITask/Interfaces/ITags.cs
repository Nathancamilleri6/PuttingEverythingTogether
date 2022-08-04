using ProjectApp.Domain;
using SimpleAPITask.DTOs;
using System.Collections.Generic;

namespace SimpleAPITask.Interfaces
{
    public interface ITags
    {
        public void AddTag(TagInputDTO tag);
        public List<Tag> GetTags();
    }
}
