using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectApp.Domain
{
    public class Project
    {
        [Required, Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [ForeignKey("Creator")]
        public int CreatorId { get; set; }
        public User? Creator { get; set; }
    }
}
