namespace PreferenceDTO
{
    public interface IProjectPreference
    {
        public int ProjectId { get; set; }
        public int UserId { get; set; }
    }

    public class ProjectPreference : IProjectPreference
    {
        public int ProjectId { get; set; }
        public int UserId { get; set; }
    }
}
