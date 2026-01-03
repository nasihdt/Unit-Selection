using UniversityRegistration.Api.Models;
using UniversityRegistration.Api.Models.DTOs;
using UniversityRegistration.Api.Repository.Interfaces;
using UniversityRegistration.Api.Services.Interfaces;

namespace UniversityRegistration.Api.Services.Implementations
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly ICourseEnrollmentRepository _enrollmentRepo;
        private readonly ICourseRepository _courseRepo;
        private readonly IStudentRepository _studentRepo;
        private readonly ICoursePrerequisiteService _prereqService;
        private readonly IRegistrationSettingsRepository _settingsRepo;

        public EnrollmentService(
            ICourseEnrollmentRepository enrollmentRepo,
            ICourseRepository courseRepo,
            IStudentRepository studentRepo,
            ICoursePrerequisiteService prereqService,
            IRegistrationSettingsRepository settingsRepo)
        {
            _enrollmentRepo = enrollmentRepo;
            _courseRepo = courseRepo;
            _studentRepo = studentRepo;
            _prereqService = prereqService;
            _settingsRepo = settingsRepo;
        }

        public async Task SelectCourseAsync(int studentId, int courseId)
        {
            var student = await _studentRepo.GetByIdAsync(studentId);
            if (student == null)
                throw new Exception("دانشجو یافت نشد");

            // ✅ باید Course همراه Sessions گرفته شود
            var course = await _courseRepo.GetByIdWithSessionsAsync(courseId);
            if (course == null)
                throw new Exception("درس یافت نشد");

            if (course.Sessions == null || course.Sessions.Count == 0)
                throw new Exception("برای این درس هیچ جلسه‌ای تعریف نشده است");

            // ✅ جلوگیری از اخذ تکراری
            if (await _enrollmentRepo.ExistsAsync(studentId, courseId))
                throw new Exception("این درس قبلاً اخذ شده است");

            // ✅ بررسی ظرفیت
            var enrolledCount = await _enrollmentRepo.CountByCourseAsync(courseId);
            if (enrolledCount >= course.Capacity)
                throw new Exception("ظرفیت درس تکمیل شده است");

            // ✅ بررسی سقف واحد
            var settings = await _settingsRepo.GetAsync();
            var currentEnrollments = await _enrollmentRepo.GetByStudentAsync(studentId);

            var currentUnits = currentEnrollments.Sum(x => x.Course.Units);
            if (settings != null && currentUnits + course.Units > settings.MaxUnits)
                throw new Exception("مجموع واحدهای انتخابی بیشتر از حد مجاز است");

            // ✅ بررسی پیش‌نیازها
            var prereqIds = await _prereqService.GetPrerequisiteIdsAsync(courseId);
            var passedCourseIds = currentEnrollments.Select(x => x.CourseId).ToList();

            if (prereqIds.Except(passedCourseIds).Any())
                throw new Exception("پیش‌نیاز این درس پاس نشده است");

            // ✅ تداخل زمانی (بین Sessions)
            foreach (var e in currentEnrollments)
            {
                var otherCourse = e.Course;

                if (otherCourse.Sessions == null || otherCourse.Sessions.Count == 0)
                    continue;

                foreach (var newSession in course.Sessions)
                {
                    foreach (var otherSession in otherCourse.Sessions)
                    {
                        if (IsSessionOverlap(newSession, otherSession))
                        {
                            throw new Exception(
                                $"تداخل زمانی با درس اخذ شده‌ی «{otherCourse.Title} ({otherCourse.Code}-{otherCourse.GroupNumber})» وجود دارد"
                            );
                        }
                    }
                }
            }

            // ✅ ثبت نهایی انتخاب واحد
            var enrollment = new CourseEnrollment
            {
                StudentId = studentId,
                CourseId = courseId
            };

            await _enrollmentRepo.AddAsync(enrollment);
        }

        public async Task RemoveCourseAsync(int studentId, int courseId)
        {
            var enrollments = await _enrollmentRepo.GetByStudentAsync(studentId);
            var enrollment = enrollments.FirstOrDefault(x => x.CourseId == courseId);

            if (enrollment == null)
                throw new Exception("این درس قبلاً اخذ نشده است");

            await _enrollmentRepo.RemoveAsync(enrollment);
        }

        // ✅ این متد دیگه Entity برنمی‌گردونه (برای جلوگیری از Cycle)
        public async Task<List<StudentEnrollmentResponse>> GetStudentEnrollmentsAsync(int studentId)
        {
            var enrollments = await _enrollmentRepo.GetByStudentAsync(studentId);

            return enrollments.Select(e => new StudentEnrollmentResponse
            {
                EnrollmentId = e.Id,
                CourseId = e.CourseId,

                Course = new CourseResponse
                {
                    Id = e.Course.Id,
                    Title = e.Course.Title,
                    Code = e.Course.Code,
                    Units = e.Course.Units,
                    GroupNumber = e.Course.GroupNumber,
                    Capacity = e.Course.Capacity,
                    TeacherName = e.Course.TeacherName,

                    // ✅ Time از Sessions محاسبه می‌شود (نه از DB)
                    Time = BuildTimeStringFromCourseSessions(e.Course.Sessions),

                    ExamDateTime = e.Course.ExamDateTime,

                    Sessions = e.Course.Sessions
                        .OrderBy(s => s.DayOfWeek)
                        .ThenBy(s => s.StartTime)
                        .Select(s => new CourseSessionDto
                        {
                            DayOfWeek = (int)s.DayOfWeek,
                            StartTime = s.StartTime,
                            EndTime = s.EndTime,
                            Location = s.Location
                        })
                        .ToList()
                }
            }).ToList();
        }

        // ==========================
        // Helpers
        // ==========================
        private static bool IsSessionOverlap(CourseSession a, CourseSession b)
        {
            if (a.DayOfWeek != b.DayOfWeek)
                return false;

            // overlap: startA < endB && startB < endA
            return a.StartTime < b.EndTime && b.StartTime < a.EndTime;
        }

        private static string GetDayName(WeekDay dayOfWeek)
        {
            return dayOfWeek switch
            {
                WeekDay.Saturday => "شنبه",
                WeekDay.Sunday => "یکشنبه",
                WeekDay.Monday => "دوشنبه",
                WeekDay.Tuesday => "سه‌شنبه",
                WeekDay.Wednesday => "چهارشنبه",
                WeekDay.Thursday => "پنجشنبه",
                WeekDay.Friday => "جمعه",
                _ => "نامشخص"
            };
        }

        private static string BuildTimeStringFromCourseSessions(ICollection<CourseSession> sessions)
        {
            if (sessions == null || sessions.Count == 0)
                return "";

            var parts = sessions
                .OrderBy(s => s.DayOfWeek)
                .ThenBy(s => s.StartTime)
                .Select(s =>
                {
                    var dayName = GetDayName(s.DayOfWeek);
                    return $"{dayName} {s.StartTime:hh\\:mm}-{s.EndTime:hh\\:mm} ({s.Location.Trim()})";
                });

            return string.Join(" | ", parts);
        }
    }
}
