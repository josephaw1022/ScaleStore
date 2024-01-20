using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceScalingCore
{

	public interface IApplicationsGetManyRequest
	{
		int ProjectId { get; }
	}

	public interface IApplicationsGetManyResponse
	{
		int Id { get; set; }
		string Name { get; set; }
		int ProjectId { get; set; }
	}

}