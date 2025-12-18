namespace UniversityRegistration.Api.Services.Interfaces
{
    public interface ICoursePrerequisiteService
    {
        Task<List<int>> GetPrerequisiteIdsAsync(int courseId);
        Task AddAsync(int courseId, int prerequisiteCourseId);
        Task RemoveAsync(int courseId, int prerequisiteCourseId);
    }
}
