namespace UniversityRegistration.Api.Models.DTOs
{
    public class CourseQueryParameters
    {
        public string? Search { get; set; }   
        public string? Code { get; set; }       
        public string? TeacherName { get; set; }  
        public int? Units { get; set; }       
        public int? MinCapacity { get; set; }   
        public bool? OnlyAvailable { get; set; }  

    }
}
