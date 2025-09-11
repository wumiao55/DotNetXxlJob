using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace DotNetXxlJob.Model
{
  
    [DataContract]
    public class KillRequest
    {

        [DataMember(Name = "jobId", Order = 1)]
        public int JobId { get; set; }
    }
}
