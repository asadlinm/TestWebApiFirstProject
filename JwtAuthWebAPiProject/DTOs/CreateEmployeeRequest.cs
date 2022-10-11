using System.ComponentModel.DataAnnotations;

namespace JwtAuthWebAPiProject.DTOs
{
    public class CreateEmployeeRequest
    {
        [Required(ErrorMessage = "Name  is required")]
        [MinLength(3)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Surname  is required")]
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        [Required(ErrorMessage = "Position field is required")]
        public string Position { get; set; }
    }
}
