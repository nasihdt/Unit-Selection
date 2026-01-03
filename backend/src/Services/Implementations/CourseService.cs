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
            var courses = await _repo.GetAllWithSessionsAsync();
            return courses.Select(MapToResponse).ToList();
        }

        public async Task<CourseResponse?> GetByIdAsync(int id)
        {
            var course = await _repo.GetByIdWithSessionsAsync(id);
            return course == null ? null : MapToResponse(course);
        }

        public async Task<CourseResponse> AddAsync(CreateCourseRequest dto)
        {
            ValidateSessions(dto.Sessions);

            // ✅ جلوگیری از تکراری بودن کد + گروه
            var duplicate = await _repo.ExistsByCodeAndGroupAsync(dto.Code, dto.GroupNumber);
            if (duplicate)
                throw new InvalidOperationException("درس با این کد و شماره گروه قبلاً ثبت شده است.");

            // ✅ چک تداخل مکانی/زمانی برای Admin
            await EnsureNoLocationConflictsAsync(
                excludeCourseId: null,
                sessions: dto.Sessions
            );

            var course = new Course
            {
                Title = dto.Title,
                Code = dto.Code.Trim(),
                Units = dto.Units,
                GroupNumber = dto.GroupNumber,
                Capacity = dto.Capacity,
                TeacherName = dto.TeacherName,

                ExamDateTime = dto.ExamDateTime.HasValue
                    ? DateTime.SpecifyKind(dto.ExamDateTime.Value, DateTimeKind.Utc)
                    : null
            };

            foreach (var s in dto.Sessions)
            {
                course.Sessions.Add(new CourseSession
                {
                    DayOfWeek = ParseWeekDay(s.DayOfWeek),
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    Location = s.Location.Trim()
                });
            }

            var created = await _repo.AddAsync(course);

            // ✅ چون AddAsync معمولاً Sessions رو Include نمی‌کنه، برای پاسخ دقیق دوباره می‌گیریم
            var createdWithSessions = await _repo.GetByIdWithSessionsAsync(created.Id);
            return MapToResponse(createdWithSessions!);
        }

        public async Task<bool> UpdateAsync(int id, UpdateCourseRequest dto)
        {
            var existing = await _repo.GetByIdWithSessionsAsync(id);
            if (existing == null)
                return false;

            ValidateSessions(dto.Sessions);

            // ✅ جلوگیری از تکراری بودن کد + گروه (غیر از خودش)
            var duplicate = await _repo.ExistsByCodeAndGroupAsync(dto.Code, dto.GroupNumber, excludeCourseId: existing.Id);
            if (duplicate)
                throw new InvalidOperationException("درس با این کد و شماره گروه قبلاً ثبت شده است.");

            // ✅ چک تداخل مکانی/زمانی (غیر از خودش)
            await EnsureNoLocationConflictsAsync(
                excludeCourseId: existing.Id,
                sessions: dto.Sessions
            );

            existing.Title = dto.Title;
            existing.Code = dto.Code.Trim();
            existing.Units = dto.Units;
            existing.GroupNumber = dto.GroupNumber;
            existing.Capacity = dto.Capacity;
            existing.TeacherName = dto.TeacherName;

            existing.ExamDateTime = dto.ExamDateTime.HasValue
                ? DateTime.SpecifyKind(dto.ExamDateTime.Value, DateTimeKind.Utc)
                : null;

            // ✅ جایگزینی Sessions
            existing.Sessions.Clear();
            foreach (var s in dto.Sessions)
            {
                existing.Sessions.Add(new CourseSession
                {
                    DayOfWeek = ParseWeekDay(s.DayOfWeek),
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    Location = s.Location.Trim()
                });
            }

            return await _repo.UpdateAsync(existing);
        }

        public async Task<bool> PatchAsync(int id, PatchCourseRequest dto)
        {
            var course = await _repo.GetByIdWithSessionsAsync(id);
            if (course == null)
                return false;

            if (dto.Title != null)
                course.Title = dto.Title;

            if (dto.Code != null)
                course.Code = dto.Code.Trim();

            if (dto.Units.HasValue)
                course.Units = dto.Units.Value;

            if (dto.GroupNumber.HasValue)
                course.GroupNumber = dto.GroupNumber.Value;

            if (dto.Capacity.HasValue)
                course.Capacity = dto.Capacity.Value;

            if (dto.TeacherName != null)
                course.TeacherName = dto.TeacherName;

            if (dto.ExamDateTime.HasValue)
                course.ExamDateTime = DateTime.SpecifyKind(dto.ExamDateTime.Value, DateTimeKind.Utc);

            // ✅ اگر کد یا گروه تغییر کرد، تکراری بودن چک شود
            if (dto.Code != null || dto.GroupNumber.HasValue)
            {
                var duplicate = await _repo.ExistsByCodeAndGroupAsync(course.Code, course.GroupNumber, excludeCourseId: course.Id);
                if (duplicate)
                    throw new InvalidOperationException("درس با این کد و شماره گروه قبلاً ثبت شده است.");
            }

            // ✅ اگر Sessions ارسال شد، جایگزین کن + چک تداخل مکانی
            if (dto.Sessions != null)
            {
                ValidateSessions(dto.Sessions);

                await EnsureNoLocationConflictsAsync(
                    excludeCourseId: course.Id,
                    sessions: dto.Sessions
                );

                course.Sessions.Clear();

                foreach (var s in dto.Sessions)
                {
                    course.Sessions.Add(new CourseSession
                    {
                        DayOfWeek = ParseWeekDay(s.DayOfWeek),
                        StartTime = s.StartTime,
                        EndTime = s.EndTime,
                        Location = s.Location.Trim()
                    });
                }
            }

            return await _repo.UpdateAsync(course);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }

        public async Task<CourseDeleteInfoResponse?> GetDeleteInfoAsync(int courseId)
        {
            var course = await _repo.GetByIdWithSessionsAsync(courseId);
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
        // ✅ Location Conflict Check (Admin Add/Update)
        // ==========================
        private async Task EnsureNoLocationConflictsAsync(
            int? excludeCourseId,
            List<CourseSessionDto> sessions)
        {
            var locations = sessions.Select(s => s.Location).ToList();
            var coursesInLocations = await _repo.GetCoursesByLocationsAsync(locations);

            foreach (var session in sessions)
            {
                var day = ParseWeekDay(session.DayOfWeek);
                var start = session.StartTime;
                var end = session.EndTime;
                var loc = session.Location.Trim();

                foreach (var course in coursesInLocations)
                {
                    if (excludeCourseId.HasValue && course.Id == excludeCourseId.Value)
                        continue;

                    foreach (var cs in course.Sessions)
                    {
                        if (cs.Location != loc) continue;

                        if (IsTimeOverlap(day, start, end, cs.DayOfWeek, cs.StartTime, cs.EndTime))
                        {
                            throw new InvalidOperationException(
                                $"در مکان «{loc}» در این بازه زمانی، درس دیگری ثبت شده است: «{course.Title} ({course.Code}-{course.GroupNumber})»"
                            );
                        }
                    }
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
        // ✅ Helpers
        // ==========================
        private static void ValidateSessions(List<CourseSessionDto> sessions)
        {
            if (sessions == null || sessions.Count == 0)
                throw new ArgumentException("حداقل یک جلسه برای درس لازم است.");

            foreach (var s in sessions)
            {
                if (s.EndTime <= s.StartTime)
                    throw new ArgumentException("ساعت پایان باید بعد از ساعت شروع باشد.");

                if (string.IsNullOrWhiteSpace(s.Location))
                    throw new ArgumentException("مکان جلسه نمی‌تواند خالی باشد.");

                if (s.DayOfWeek < 0 || s.DayOfWeek > 6)
                    throw new ArgumentException("روز هفته نامعتبر است (0 تا 6).");
            }
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

        private static string BuildTimeStringFromCourseSessions(ICollection<CourseSession> sessions)
        {
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

        // ==========================
        // ✅ Mapping
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

                // ✅ Time همیشه از Sessions ساخته می‌شود (نه از دیتابیس)
                Time = BuildTimeStringFromCourseSessions(course.Sessions),

                ExamDateTime = course.ExamDateTime,

                Sessions = course.Sessions
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
            };
        }
    }
}
