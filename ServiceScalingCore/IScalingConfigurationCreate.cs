using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceScalingCore
{
	public interface ICreateScalingConfigurationRequest
	{
		int ApplicationID { get; set; }
		int EnvironmentID { get; set; }
		int NumberOfInstances { get; set; }
	}

	public interface IScalingConfigurationResponse
	{
		int ScalingID { get; set; }
		int ApplicationID { get; set; }
		int EnvironmentID { get; set; }
		int NumberOfInstances { get; set; }
	}


}