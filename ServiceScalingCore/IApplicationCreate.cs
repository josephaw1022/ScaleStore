namespace ServiceScalingCore
{
    public interface IApplicationCreate
    {
        public string Name { get; set;  }
        public int ProjectId { get; set; }
    }



    public interface IApplicationCreateResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProjectId { get; set; }
    }
}
