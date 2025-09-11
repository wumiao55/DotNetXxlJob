using System.Runtime.Serialization;
using DotNetXxlJob.Internal;

namespace DotNetXxlJob.Model
{
    [DataContract(Name = Constants.RegistryParamJavaFullName)]
    public class RegistryParam
    {
        /// <summary>
        /// �̶�ֵ:EXECUTOR
        /// </summary>
        [DataMember(Name = "registryGroup", Order = 1)]
        public string RegistryGroup { get; set; } = "EXECUTOR";

        /// <summary>
        /// ִ����AppName
        /// </summary>
        [DataMember(Name = "registryKey", Order = 2)]
        public string RegistryKey { get; set; } = string.Empty;

        /// <summary>
        /// ִ������ַ�����÷������ַ
        /// </summary>
        [DataMember(Name = "registryValue", Order = 3)]
        public string RegistryValue { get; set; } = string.Empty;
    }
}