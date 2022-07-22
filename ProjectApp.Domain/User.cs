using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectApp.Domain
{
    public class User
    {
        [Required, Key]
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateTime CreatedDate { get; set; }

        [InverseProperty("Assignee")]
        public virtual ICollection<Project_Assignee>? Assignees { get; set; }
        [InverseProperty("Creator")]
        public virtual ICollection<Project>? Creators { get; set; }
        [InverseProperty("Commentator")]
        public virtual ICollection<Comment>? Commentators { get; set; }
    }
}
