using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceScalingDTO
{

	public class CreateEnvironmentDTO
	{
		public string EnvironmentName { get; set; } = null!;
		public int ProjectID { get; set; }
	}

	public class UpdateEnvironmentDTO
	{
		public int EnvironmentID { get; set; }
		public string EnvironmentName { get; set; } = null!;
		public int ProjectID { get; set; }
	}
}