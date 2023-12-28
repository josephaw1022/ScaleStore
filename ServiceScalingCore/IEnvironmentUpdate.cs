namespace ServiceScalingCore
{
    public interface IUpdateEnvironmentRequest
    {
        int EnvironmentID { get; set; }
        string EnvironmentName { get; set; }
        int ProjectID { get; set; }
    }

    public interface IUpdateEnvironmentResponse
    {
        bool Success { get; set; }
    }
}
