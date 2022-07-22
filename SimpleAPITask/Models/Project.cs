using System;
using System.Collections.Generic;

namespace SimpleAPITask.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Comments { get; set; }

        public Project(int Id, string Name, List<string> Comments)
        {
            this.Id = Id;
            this.Name = Name;
            this.Comments = Comments;
        }
        public Project()
        {
        }
    }
}
