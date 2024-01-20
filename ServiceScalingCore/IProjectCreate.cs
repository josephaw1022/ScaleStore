using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceScalingCore
{
	public interface ICreateProjectRequest
	{
		string Name { get; set; }
	}

	public interface ICreateProjectResponse
	{
		int Id { get; set; }
		string Name { get; set; }
	}
}