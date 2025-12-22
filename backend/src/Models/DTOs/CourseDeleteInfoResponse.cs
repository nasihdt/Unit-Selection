namespace UniversityRegistration.Api.Models.DTOs
{
    public class CourseDeleteInfoResponse
    {
        public int CourseId { get; set; }

        public bool HasPrerequisites { get; set; }

        public bool IsPrerequisiteForOthers { get; set; }

        public int DependentCoursesCount { get; set; }
    }
}

