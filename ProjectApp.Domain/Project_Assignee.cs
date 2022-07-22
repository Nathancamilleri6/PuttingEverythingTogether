using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectApp.Domain
{
    public class Project_Assignee
    {
        [Required, Key]
        public int Id { get; set; }
        [ForeignKey("Project")]
        public int ProjectId { get; set; }
        public Project? Project { get; set; }
        [ForeignKey("Assignee")]
        public int? AssigneeId { get; set; }
        public User? Assignee { get; set; }
    }
}
