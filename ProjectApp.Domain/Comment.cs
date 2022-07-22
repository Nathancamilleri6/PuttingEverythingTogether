using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectApp.Domain
{
    public class Comment
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string? Value { get; set; }
        [ForeignKey("Commentator")]
        public int CommentatorId { get; set; }
        public User? Commentator { get; set; }
        [ForeignKey("Project")]
        public int ProjectId { get; set; }
        public Project? Project { get; set; }
    }
}
