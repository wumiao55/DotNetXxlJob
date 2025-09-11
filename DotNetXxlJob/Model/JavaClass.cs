using System.Runtime.Serialization;
using DotNetXxlJob.Internal;

namespace DotNetXxlJob.Model
{
    [DataContract(Name = Constants.JavaClassFulName)]
    public class JavaClass
    {
        [DataMember(Name = "name",Order = 1)]
        public string Name { get; set; }
    }
}