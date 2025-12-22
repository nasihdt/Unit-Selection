namespace UniversityRegistration.Api.Models
{
    public class Professor
    {
        public int Id { get; set; }

        public string ProfessorCode { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Role { get; set; } = "Professor";
    }
}
