using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceScalingCore
{
    public interface IProjectsTableViewRequest
    {
        // This interface may not have any members if the request does not carry any data.
    }

    public interface IProjectTableViewResponse
    {
        int Id { get; set; }
        string Name { get; set; }
        int NumberOfEnvironments { get; set; }
        int NumberOfApplications { get; set; }
    }

}
