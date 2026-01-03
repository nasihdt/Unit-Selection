namespace UniversityRegistration.Api.Models
{
    public class CourseSession
    {
        public int Id { get; set; }

        // ارتباط با درس
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public WeekDay DayOfWeek { get; set; }

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public string Location { get; set; } = null!;
    }
}

