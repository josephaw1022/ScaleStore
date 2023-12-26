namespace ServiceScalingDb.ScalingDb;

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Project
{
    [Key]
    public int ProjectID { get; set; }
    public string ProjectName { get; set; } = null!;

    public ICollection<Environment> Environments { get; set; } = new List<Environment>();
    public ICollection<Application> Applications { get; set; } = new List<Application>();
}

public class Environment
{
    [Key]
    public int EnvironmentID { get; set; }
    public string EnvironmentName { get; set; } = null!;
    public int ProjectID { get; set; }

    [ForeignKey("ProjectID")]
    public Project Project { get; set; } = null!;
    public ICollection<ScalingConfiguration> ScalingConfigurations { get; set; } = new List<ScalingConfiguration>();
}

public class Application
{
    [Key]
    public int ApplicationID { get; set; }
    public string ApplicationName { get; set; } = null!;
    public int ProjectID { get; set; }

    [ForeignKey("ProjectID")]
    public Project Project { get; set; } = null!;
    public ICollection<ScalingConfiguration> ScalingConfigurations { get; set; } = new List<ScalingConfiguration>();
}

public class ScalingConfiguration
{
    [Key]
    public int ScalingID { get; set; }
    public int ApplicationID { get; set; }
    public int EnvironmentID { get; set; }

    [DefaultValue(1)]
    public int NumberOfInstances { get; set; } = 1;

    [ForeignKey("ApplicationID")]
    public Application Application { get; set; } = null!;

    [ForeignKey("EnvironmentID")]
    public Environment Environment { get; set; } = null!;

}