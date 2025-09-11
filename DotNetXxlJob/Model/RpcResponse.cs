using System.Runtime.Serialization;
using DotNetXxlJob.Internal;

namespace DotNetXxlJob.Model
{
    [DataContract(Name = Constants.RpcResponseJavaFullName)]
    public class RpcResponse
    {
        [DataMember(Name = "requestId",Order = 1)]
        public string RequestId{ get; set; }
        [DataMember(Name = "errorMsg",Order = 2)]
        public string ErrorMsg { get; set; }
        [DataMember(Name = "result",Order = 3)]
        public object Result{ get; set; }
      
        
        public bool IsError => this.ErrorMsg != null;
    }
}