using UniversityRegistration.Api.Models;
using UniversityRegistration.Api.Models.DTOs;
using UniversityRegistration.Api.Repository.Interfaces;
using UniversityRegistration.Api.Services.Interfaces;

namespace UniversityRegistration.Api.Services.Implementations
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _repo;
        private readonly ICoursePrerequisiteRepository _prereqRepo;

        public CourseService(ICourseRepository repo, ICoursePrerequisiteRepository prereqRepo)
        {
            _repo = repo;
            _prereqRepo = prereqRepo;
        }

        public async Task<List<CourseResponse>> GetAllAsync()
        {
            var courses = await _repo.GetAllAsync();
            return courses.Select(MapToResponse).ToList();
        }

        public async Task<CourseResponse?> GetByIdAsync(int id)
        {
            var course = await _repo.GetByIdAsync(id);
            return course == null ? null : MapToResponse(course);
        }

        public async Task<CourseResponse> AddAsync(CreateCourseRequest dto)
        {
            var day = ParseWeekDay(dto.DayOfWeek);
            ValidateSchedule(dto.StartTime, dto.EndTime);

            //  چک تداخل مکانی برای Admin هنگام ثبت درس
            await EnsureNoLocationConflictAsync(
                excludeCourseId: null,
                location: dto.Location,
                dayOfWeek: day,
                startTime: dto.StartTime,
                endTime: dto.EndTime
            );

            var course = new Course
            {
                Title = dto.Title,
                Code = dto.Code,
                Units = dto.Units,
                GroupNumber = dto.GroupNumber,
                Capacity = dto.Capacity,
                TeacherName = dto.TeacherName,
                Location = dto.Location,

                DayOfWeek = day,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Time = BuildTimeString(day, dto.StartTime, dto.EndTime),

                ExamDateTime = dto.ExamDateTime.HasValue
                    ? DateTime.SpecifyKind(dto.ExamDateTime.Value, DateTimeKind.Utc)
                    : null
            };

            var created = await _repo.AddAsync(course);
            return MapToResponse(created);
        }

        public async Task<bool> UpdateAsync(int id, UpdateCourseRequest dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
                return false;

            var day = ParseWeekDay(dto.DayOfWeek);
            ValidateSchedule(dto.StartTime, dto.EndTime);

            //  چک تداخل مکانی برای Admin هنگام ویرایش درس
            await EnsureNoLocationConflictAsync(
                excludeCourseId: existing.Id,
                location: dto.Location,
                dayOfWeek: day,
                startTime: dto.StartTime,
                endTime: dto.EndTime
            );

            existing.Title = dto.Title;
            existing.Code = dto.Code;
            existing.Units = dto.Units;
            existing.GroupNumber = dto.GroupNumber;
            existing.Capacity = dto.Capacity;
            existing.TeacherName = dto.TeacherName;

            existing.Location = dto.Location;
            existing.DayOfWeek = day;
            existing.StartTime = dto.StartTime;
            existing.EndTime = dto.EndTime;
            existing.Time = BuildTimeString(day, dto.StartTime, dto.EndTime);

            existing.ExamDateTime = dto.ExamDateTime.HasValue
                ? DateTime.SpecifyKind(dto.ExamDateTime.Value, DateTimeKind.Utc)
                : null;

            return await _repo.UpdateAsync(existing);
        }

        public async Task<bool> PatchAsync(int id, PatchCourseRequest dto)
        {
            var course = await _repo.GetByIdAsync(id);
            if (course == null)
                return false;

            if (dto.Title != null)
                course.Title = dto.Title;

            if (dto.Code != null)
                course.Code = dto.Code;

            if (dto.Units.HasValue)
                course.Units = dto.Units.Value;

            if (dto.GroupNumber.HasValue)
                course.GroupNumber = dto.GroupNumber.Value;

            if (dto.Capacity.HasValue)
                course.Capacity = dto.Capacity.Value;

            if (dto.TeacherName != null)
                course.TeacherName = dto.TeacherName;

            if (dto.Location != null)
                course.Location = dto.Location;

            if (dto.ExamDateTime.HasValue)
            {
                course.ExamDateTime = DateTime.SpecifyKind(
                    dto.ExamDateTime.Value,
                    DateTimeKind.Utc
                );
            }

            //  اگر برنامه‌ی زمانی عوض شد باید Time دوباره ساخته شود
            bool scheduleChanged = false;

            if (dto.DayOfWeek.HasValue)
            {
                course.DayOfWeek = ParseWeekDay(dto.DayOfWeek.Value);
                scheduleChanged = true;
            }

            if (dto.StartTime.HasValue)
            {
                course.StartTime = dto.StartTime.Value;
                scheduleChanged = true;
            }

            if (dto.EndTime.HasValue)
            {
                course.EndTime = dto.EndTime.Value;
                scheduleChanged = true;
            }

            if (scheduleChanged)
            {
                ValidateSchedule(course.StartTime, course.EndTime);
                course.Time = BuildTimeString(course.DayOfWeek, course.StartTime, course.EndTime);
            }

            //  (چک تداخل مکانی فقط وقتی لازمه (مکان یا زمان تغییر کرده
            if (dto.Location != null || scheduleChanged)
            {
                await EnsureNoLocationConflictAsync(
                    excludeCourseId: course.Id,
                    location: course.Location,
                    dayOfWeek: course.DayOfWeek,
                    startTime: course.StartTime,
                    endTime: course.EndTime
                );
            }

            return await _repo.UpdateAsync(course);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }

        public async Task<CourseDeleteInfoResponse?> GetDeleteInfoAsync(int courseId)
        {
            var course = await _repo.GetByIdAsync(courseId);
            if (course == null)
                return null;

            var hasPrerequisites = await _prereqRepo.HasPrerequisitesAsync(courseId);
            var dependentCount = await _prereqRepo.DependentCoursesCountAsync(courseId);

            return new CourseDeleteInfoResponse
            {
                CourseId = courseId,
                HasPrerequisites = hasPrerequisites,
                IsPrerequisiteForOthers = dependentCount > 0,
                DependentCoursesCount = dependentCount
            };
        }

        public async Task<List<CourseResponse>> GetFilteredAsync(CourseQueryParameters q)
        {
            var courses = await _repo.GetFilteredAsync(q);
            return courses.Select(MapToResponse).ToList();
        }

        // ==========================
        // Location Conflict
        // ==========================
        private async Task EnsureNoLocationConflictAsync(
            int? excludeCourseId,
            string location,
            WeekDay dayOfWeek,
            TimeSpan startTime,
            TimeSpan endTime)
        {
            var coursesInLocation = await _repo.GetCoursesByLocationAsync(location);

            foreach (var c in coursesInLocation)
            {
                if (excludeCourseId.HasValue && c.Id == excludeCourseId.Value)
                    continue;

                // اگر درس قبلی هنوز schedule ندارد، ردش می‌کنیم
                // (در حالت migration قدیمی یا دیتای ناقص)
                // اگر همه required هستند، این قسمت عملاً لازم نیست
                if (c.StartTime == default || c.EndTime == default)
                    continue;

                if (IsTimeOverlap(dayOfWeek, startTime, endTime, c.DayOfWeek, c.StartTime, c.EndTime))
                {
                    throw new Exception(
                        $"در مکان «{location}» در این بازه زمانی، درس دیگری ثبت شده است: «{c.Title} ({c.Code}-{c.GroupNumber})»"
                    );
                }
            }
        }

        private static bool IsTimeOverlap(
            WeekDay day1, TimeSpan start1, TimeSpan end1,
            WeekDay day2, TimeSpan start2, TimeSpan end2)
        {
            if (day1 != day2) return false;
            return start1 < end2 && start2 < end1;
        }

        // ==========================
        // Helpers
        // ==========================
        private static void ValidateSchedule(TimeSpan start, TimeSpan end)
        {
            if (end <= start)
                throw new ArgumentException("ساعت پایان باید بعد از ساعت شروع باشد.");
        }

        private static WeekDay ParseWeekDay(int dayOfWeek)
        {
            if (dayOfWeek < 0 || dayOfWeek > 6)
                throw new ArgumentException("روز هفته نامعتبر است (0 تا 6).");

            return (WeekDay)dayOfWeek;
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

        private static string BuildTimeString(WeekDay dayOfWeek, TimeSpan start, TimeSpan end)
        {
            var dayName = GetDayName(dayOfWeek);
            return $"{dayName} {start:hh\\:mm}-{end:hh\\:mm}";
        }

        // ==========================
        // Mapping
        // ==========================
        private static CourseResponse MapToResponse(Course course)
        {
            return new CourseResponse
            {
                Id = course.Id,
                Title = course.Title,
                Code = course.Code,
                Units = course.Units,
                GroupNumber = course.GroupNumber,
                Capacity = course.Capacity,
                TeacherName = course.TeacherName,

                Time = course.Time,
                DayOfWeek = (int)course.DayOfWeek,
                StartTime = course.StartTime,
                EndTime = course.EndTime,

                Location = course.Location,
                ExamDateTime = course.ExamDateTime
            };
        }
    }
}
