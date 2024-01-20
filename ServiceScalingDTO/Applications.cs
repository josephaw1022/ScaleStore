namespace ServiceScalingDTO
{
	public class CreateApplicationDTO
	{
		public string Name { get; set; } = null!;
		public int ProjectId { get; set; }
	}



	public class UpdateApplicationDTO
	{
		public int Id { get; set; }
		public string Name { get; set; } = null!;
		public int ProjectId { get; set; }
	}
}