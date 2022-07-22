using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectApp.Domain
{
    public class Project_Tag
    {
        [Required, Key]
        public int Id { get; set; }
        [ForeignKey("Project")]
        public int ProjectId { get; set; }
        public Project? Project { get; set; }
        [ForeignKey("Tag")]
        public int TagId { get; set; }
        public Tag? Tag { get; set; }
    }
}
