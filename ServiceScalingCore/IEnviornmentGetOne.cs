namespace ServiceScalingCore
{
    public interface IGetEnvironmentRequest
    {
        int EnvironmentID { get; set; }
    }

    public interface IGetEnvironmentResponse
    {
        int EnvironmentID { get; set; }
        string EnvironmentName { get; set; }
        int ProjectID { get; set; }
    }
}
