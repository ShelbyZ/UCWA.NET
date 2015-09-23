using System.Runtime.Serialization;

namespace UCWA.NET.Resources
{
    [DataContract]
    public class Applications : Resource
    {
        [DataMember(Name = "culture")]
        public string Culture { get; set; }

        [DataMember(Name = "endpointId")]
        public string EndpointId { get; set; }

        [DataMember(Name = "instanceId", IsRequired = false, EmitDefaultValue = false)]
        public string InstanceId { get; set; }

        [DataMember(Name = "userAgent")]
        public string UserAgent { get; set; }
    }
}
