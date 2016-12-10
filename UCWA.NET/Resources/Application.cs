using System.Runtime.Serialization;

namespace UCWA.NET.Resources
{
    [DataContract]
    public class Application : Resource
    {
        [DataContract]
        public class LinksObject
        {
            [DataMember(Name = "self")]
            public HrefObject Self { get; set; }

            [DataMember(Name = "policies")]
            public HrefObject Policies { get; set; }

            [DataMember(Name = "batch")]
            public HrefObject Batch { get; set; }

            [DataMember(Name = "events")]
            public HrefObject Events { get; set; }
        }

        [DataContract]
        public class EmbeddedObject
        {
            [DataMember(Name = "me")]
            public Me Me { get; set; }

            [DataMember(Name = "people")]
            public People People { get; set; }

            [DataMember(Name = "onlineMeetings")]
            public OnlineMeetings OnlineMeetings { get; set; }

            [DataMember(Name = "communication")]
            public Communication Communication { get; set; }
        }

        [DataMember(Name = "culture")]
        public string Culture { get; set; }

        [DataMember(Name = "endpointId")]
        public string EndpointId { get; set; }

        [DataMember(Name = "instanceId", IsRequired = false, EmitDefaultValue = false)]
        public string InstanceId { get; set; }

        [DataMember(Name = "userAgent")]
        public string UserAgent { get; set; }

        [DataMember(Name = "_links", IsRequired = false, EmitDefaultValue = false)]
        public LinksObject Links { get; set; }

        [DataMember(Name = "_embedded", IsRequired = false, EmitDefaultValue = false)]
        public EmbeddedObject Embedded { get; set; }

        [DataMember(Name = "rel", IsRequired = false, EmitDefaultValue = false)]
        public string Rel { get; set; }

        [DataMember(Name = "etag", IsRequired = false, EmitDefaultValue = false)]
        public string ETag { get; set; }
    }
}
