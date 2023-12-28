using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceScalingCore
{
    public interface IUpdateApplicationRequest
    {
        int Id { get; set; }
        string Name { get; set; }
        int ProjectId { get; set; }
    }

    public interface IUpdateApplicationResponse
    {
        int Id { get; set; }
        string Name { get; set; }
        int ProjectId { get; set; }
        bool Success { get; set; }
    }

}
