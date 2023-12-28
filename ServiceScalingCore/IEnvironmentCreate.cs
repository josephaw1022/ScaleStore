namespace ServiceScalingCore
{
    public interface ICreateEnvironmentRequest
    {
        string EnvironmentName { get; set; }
        int ProjectID { get; set; }
    }

    public interface ICreateEnvironmentResponse
    {
        int EnvironmentID { get; set; }
        string EnvironmentName { get; set; }
        int ProjectID { get; set; }
    }

}
