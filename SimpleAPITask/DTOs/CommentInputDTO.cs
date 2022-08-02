using System;

namespace SimpleAPITask.DTOs
{
    public class CommentInputDTO
    {
        public DateTime DateTime { get; set; }
        public string Value { get; set; }
        public int CommentatorId { get; set; }
        public int ProjectId { get; set; }
    }
}
