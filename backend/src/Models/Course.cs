namespace UniversityRegistration.Api.Models
{
    public class Course
    {
        public int Id { get; set; }

        // اسم درس
        public string Title { get; set; } = null!;

        // کد درس
        public string Code { get; set; } = null!;

        // تعداد واحد
        public int Units { get; set; }

        // ظرفیت
        public int Capacity { get; set; }

        // نام استاد
        public string TeacherName { get; set; } = null!;

        // زمان برگزاری (مثلاً "شنبه 10-12")
        public string Time { get; set; } = null!;

        // مکان برگزاری (مثلاً "کلاس 201")
        public string Location { get; set; } = null!;
        public ICollection<CoursePrerequisite> Prerequisites { get; set; } = new List<CoursePrerequisite>();
        public ICollection<CoursePrerequisite> IsPrerequisiteFor { get; set; } = new List<CoursePrerequisite>();


        // توضیحات اختیاری
        public string? Description { get; set; }
    }
}
    