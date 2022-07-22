using System.ComponentModel.DataAnnotations;

namespace ProjectApp.Domain
{
    public class Tag
    {
        [Required, Key]
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
