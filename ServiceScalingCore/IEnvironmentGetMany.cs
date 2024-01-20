namespace ServiceScalingCore
{
	public interface IGetManyEnvironmentsRequest
	{
		int ProjectID { get; set; }
	}

	public interface IGetManyEnvironmentsResponse
	{
		int EnvironmentID { get; set; }
		string EnvironmentName { get; set; }
		string ProjectName { get; set; }
	}

}