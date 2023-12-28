using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceScalingCore
{
    public interface IGetManyScalingConfigurationsRequest
    {
        int ProjectID { get; set; }
    }


    public interface IScalingConfigurationTableViewResponse
    {
        public int Id { get; set; }
        public string EnvironmentName { get; set; }
        public string ApplicationName { get; set; }
        public int NumberOfInstances { get; set; }
    }

}
