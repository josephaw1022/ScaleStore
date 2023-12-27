namespace ServiceScalingDTO
{
    public class ProjectCreateDTO
    {
        public string Name { get; set; } = null!;
    }


    public class ProjectUpdateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
