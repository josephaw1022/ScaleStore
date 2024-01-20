using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceScalingCore
{
	public interface IProjectsGetOneRequest
	{
		int Id { get; set; }
	}

	public interface IProjectsGetOneResponse
	{
		int Id { get; set; }
		string Name { get; set; }
	}

}