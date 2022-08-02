using ProjectApp.Domain;
using SimpleAPITask.DTOs;
using System.Collections.Generic;

namespace SimpleAPITask.Interfaces
{
    public interface ITags
    {
        public List<Tag> GetTags();
        public void AddTag(TagInputDTO tag);
    }
}
