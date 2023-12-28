namespace ServiceScalingCore
{
    public interface IApplicationGetOneRequest
    {
        int Id { get; }
    }

    public interface IApplicationGetOneResponse
    {
        int Id { get; set; }
        string Name { get; set; }
        int ProjectId { get; set; }
    }

}
