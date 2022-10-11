using System.ComponentModel.DataAnnotations;

namespace JwtAuthWebAPiProject.DTOs
{
    public class CreateUserRequest
    {
        [Required]
        [EmailAddress]
        [MinLength(10)]
        public string Email { get; set; }
        [Required]
        [MinLength(5)]
        public string Password { get; set; }
    }
}
