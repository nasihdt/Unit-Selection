namespace UniversityRegistration.Api.Models
{
    public class RegistrationSettings
    {
        public int Id { get; set; }  // یک رکورد (Id=1)

        public int MinUnits { get; set; }   // حداقل واحد
        public int MaxUnits { get; set; }   // حداکثر واحد
    }
}
