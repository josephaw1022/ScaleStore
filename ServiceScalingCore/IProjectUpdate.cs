using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceScalingCore
{
	public interface IUpdateProjectRequest
	{
		int Id { get; set; }
		string Name { get; set; }
	}

	public interface IUpdateProjectResponse
	{
		int Id { get; set; }
		string Name { get; set; }
		bool IsSuccess { get; set; }
	}
}