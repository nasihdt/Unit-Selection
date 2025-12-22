using UniversityRegistration.Api.Models;
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

        // =========================
        // Select Course (اخذ درس)
        // =========================
        public async Task SelectCourseAsync(int studentId, int courseId)
        {
            var student = await _studentRepo.GetByStudentNumberAsync(studentId.ToString());
            if (student == null)
                throw new Exception("دانشجو یافت نشد");

            var course = await _courseRepo.GetByIdAsync(courseId);
            if (course == null)
                throw new Exception("درس یافت نشد");

            // جلوگیری از اخذ تکراری
            if (await _enrollmentRepo.ExistsAsync(studentId, courseId))
                throw new Exception("این درس قبلاً اخذ شده است");

            // بررسی ظرفیت
            var enrolledCount = await _enrollmentRepo.CountByCourseAsync(courseId);
            if (enrolledCount >= course.Capacity)
                throw new Exception("ظرفیت درس تکمیل شده است");

            // بررسی سقف واحد
            var settings = await _settingsRepo.GetAsync();
            var currentEnrollments = await _enrollmentRepo.GetByStudentAsync(studentId);
            var currentUnits = currentEnrollments.Sum(x => x.Course.Units);

            if (settings != null && currentUnits + course.Units > settings.MaxUnits)
                throw new Exception("مجموع واحدهای انتخابی بیشتر از حد مجاز است");

            // بررسی پیش‌نیازها (از Service استفاده می‌کنیم)
            var prereqIds = await _prereqService.GetPrerequisiteIdsAsync(courseId);
            var passedCourseIds = currentEnrollments.Select(x => x.CourseId).ToList();

            if (prereqIds.Except(passedCourseIds).Any())
                throw new Exception("پیش‌نیاز این درس پاس نشده است");

            // بررسی تداخل زمانی کلاس
            if (currentEnrollments.Any(e => e.Course.Time == course.Time))
                throw new Exception("تداخل زمانی با درس دیگر وجود دارد");

            // ثبت نهایی انتخاب واحد
            var enrollment = new CourseEnrollment
            {
                StudentId = studentId,
                CourseId = courseId
            };

            await _enrollmentRepo.AddAsync(enrollment);
        }

        // =========================
        // Remove Course (حذف درس)
        // =========================
        public async Task RemoveCourseAsync(int studentId, int courseId)
        {
            var enrollments = await _enrollmentRepo.GetByStudentAsync(studentId);
            var enrollment = enrollments.FirstOrDefault(x => x.CourseId == courseId);

            if (enrollment == null)
                throw new Exception("این درس قبلاً اخذ نشده است");

            await _enrollmentRepo.RemoveAsync(enrollment);
        }

        // =========================
        // Get Student Enrollments
        // =========================
        public async Task<List<CourseEnrollment>> GetStudentEnrollmentsAsync(int studentId)
        {
            return await _enrollmentRepo.GetByStudentAsync(studentId);
        }
    }
}
