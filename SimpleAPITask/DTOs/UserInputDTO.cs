using System;

namespace SimpleAPITask.DTOs
{
    public class UserInputDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
