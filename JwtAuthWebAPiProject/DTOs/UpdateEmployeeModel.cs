namespace JwtAuthWebAPiProject.DTOs
{
    public class UpdateEmployeeModel
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string Position { get; set; }
    }
}
