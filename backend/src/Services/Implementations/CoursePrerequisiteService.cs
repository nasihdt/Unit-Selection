using UniversityRegistration.Api.Models;
using UniversityRegistration.Api.Repository.Interfaces;
using UniversityRegistration.Api.Services.Interfaces;

namespace UniversityRegistration.Api.Services.Implementations
{
    public class CoursePrerequisiteService : ICoursePrerequisiteService
    {
        private readonly ICoursePrerequisiteRepository _repo;

        public CoursePrerequisiteService(ICoursePrerequisiteRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<int>> GetPrerequisiteIdsAsync(int courseId)
        {
            var exists = await _repo.CourseExistsAsync(courseId);
            if (!exists)
                throw new KeyNotFoundException("درس موردنظر یافت نشد.");

            return await _repo.GetPrerequisiteIdsAsync(courseId);
        }

        public async Task AddAsync(int courseId, int prerequisiteCourseId)
        {
            // یک درس نمی‌تواند پیش‌نیاز خودش باشد
            if (courseId == prerequisiteCourseId)
                throw new InvalidOperationException("یک درس نمی‌تواند پیش‌نیاز خودش باشد.");

            // وجود داشتن درس‌ها
            var courseExists = await _repo.CourseExistsAsync(courseId);
            var prereqExists = await _repo.CourseExistsAsync(prerequisiteCourseId);

            if (!courseExists || !prereqExists)
                throw new KeyNotFoundException("درس یا پیش‌نیاز موردنظر یافت نشد.");

            // جلوگیری از تکرار همان پیش‌نیاز
            var alreadyExists = await _repo.ExistsAsync(courseId, prerequisiteCourseId);
            if (alreadyExists)
                throw new InvalidOperationException("این پیش‌نیاز قبلاً برای این درس ثبت شده است.");

            // جلوگیری از پیش‌نیاز دوطرفه (A↔B)
            var reverseExists = await _repo.ExistsAsync(prerequisiteCourseId, courseId);
            if (reverseExists)
                throw new InvalidOperationException("این دو درس نمی‌توانند پیش‌نیاز یکدیگر باشند.");

            // جلوگیری از ایجاد چرخه‌های طولانی‌تر (A→B→C→A)
            var createsCycle = await WouldCreateCycleAsync(courseId, prerequisiteCourseId);
            if (createsCycle)
                throw new InvalidOperationException("این پیش‌نیاز باعث ایجاد چرخه در پیش‌نیازها می‌شود.");

            var entity = new CoursePrerequisite
            {
                CourseId = courseId,
                PrerequisiteCourseId = prerequisiteCourseId
            };

            await _repo.AddAsync(entity);
        }

        public async Task RemoveAsync(int courseId, int prerequisiteCourseId)
        {
            var entity = await _repo.GetAsync(courseId, prerequisiteCourseId);
            if (entity == null)
                throw new KeyNotFoundException("پیش‌نیاز موردنظر یافت نشد.");

            await _repo.RemoveAsync(entity);
        }

        // =====================================
        //  Cycle Detection (DFS)
        // =====================================

        private async Task<bool> WouldCreateCycleAsync(int courseId, int prerequisiteCourseId)
        {
  
            var visited = new HashSet<int>();
            return await HasPathToCourseAsync(prerequisiteCourseId, courseId, visited);
        }

        private async Task<bool> HasPathToCourseAsync(int currentCourseId, int targetCourseId, HashSet<int> visited)
        {
            if (currentCourseId == targetCourseId)
                return true;

            if (!visited.Add(currentCourseId))
                return false; 

            var prereqIds = await _repo.GetPrerequisiteIdsAsync(currentCourseId);

            foreach (var prereqId in prereqIds)
            {
                if (await HasPathToCourseAsync(prereqId, targetCourseId, visited))
                    return true;
            }

            return false;
        }
    }
}
