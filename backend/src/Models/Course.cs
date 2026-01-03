using System.ComponentModel.DataAnnotations.Schema;

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

        // شماره گروه
        public int GroupNumber { get; set; }

        // ظرفیت
        public int Capacity { get; set; }

        // نام استاد
        public string TeacherName { get; set; } = null!;

        // زمان برگزاری (برای نمایش در UI - مشتق شده از Sessions)
        // مثال: "شنبه 10:00-12:00 (کلاس 201) | دوشنبه 17:00-19:00 (کلاس 101)"
        [NotMapped]
        public string Time { get; set; } = null!;

        // جلسات برگزاری درس (برای پشتیبانی از چند جلسه در هفته)
        public ICollection<CourseSession> Sessions { get; set; } = new List<CourseSession>();

        // لیست پیش‌نیازهای این درس
        public ICollection<CoursePrerequisite> Prerequisites { get; set; } = new List<CoursePrerequisite>();

        // لیست درس‌هایی که این درس پیش‌نیاز آن‌هاست
        public ICollection<CoursePrerequisite> IsPrerequisiteFor { get; set; } = new List<CoursePrerequisite>();

        // تاریخ و زمان امتحان (اختیاری)
        public DateTime? ExamDateTime { get; set; }
    }
}
