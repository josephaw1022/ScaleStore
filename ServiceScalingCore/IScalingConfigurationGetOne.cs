using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceScalingCore
{
    public interface IGetScalingConfigurationRequest
    {
        int ScalingID { get; set; }
    }



    public interface IGetScalingConfigurationResponse
    {
        int ScalingID { get; set; }
        int ApplicationID { get; set; }
        int EnvironmentID { get; set; }
        int NumberOfInstances { get; set; }
    }

}
