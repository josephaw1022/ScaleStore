using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceScalingDTO
{


    public class CreateScalingConfigurationDTO
    {
        public int ApplicationID { get; set; }
        public int EnvironmentID { get; set; }
        public int NumberOfInstances { get; set; }

    }


    public class UpdateScalingConfigurationDTO
    {
        public int ScalingID { get; set; }
        public int ApplicationID { get; set; }
        public int EnvironmentID { get; set; }
        public int NumberOfInstances { get; set; }


    }



}
