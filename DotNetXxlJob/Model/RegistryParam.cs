using System.Runtime.Serialization;
using DotNetXxlJob.Internal;

namespace DotNetXxlJob.Model
{
    [DataContract(Name = Constants.RegistryParamJavaFullName)]
    public class RegistryParam
    {
        /// <summary>
        /// 固定值:EXECUTOR
        /// </summary>
        [DataMember(Name = "registryGroup", Order = 1)]
        public string RegistryGroup { get; set; } = "EXECUTOR";

        /// <summary>
        /// 执行器AppName
        /// </summary>
        [DataMember(Name = "registryKey", Order = 2)]
        public string RegistryKey { get; set; } = string.Empty;

        /// <summary>
        /// 执行器地址，内置服务跟地址
        /// </summary>
        [DataMember(Name = "registryValue", Order = 3)]
        public string RegistryValue { get; set; } = string.Empty;
    }
}