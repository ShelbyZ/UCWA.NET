using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UCWA.NET.Resources
{
    [DataContract]
    public class Communication : Resource
    {
        [DataContract]
        public class LinksObject
        {
            [DataMember(Name = "self")]
            public HrefObject Self { get; set; }

            [DataMember(Name = "startPhoneAudio")]
            public HrefObject StartPhoneAudio { get; set; }

            [DataMember(Name = "conversations")]
            public HrefObject Conversations { get; set; }

            [DataMember(Name = "startMessaging")]
            public HrefObject StartMessaging { get; set; }

            [DataMember(Name = "startOnlineMeeting")]
            public HrefObject StartOnlineMeeting { get; set; }

            [DataMember(Name = "joinOnlineMeeting")]
            public HrefObject JoinOnlineMeeting { get; set; }

            [DataMember(Name = "missedItems")]
            public HrefObject MissedItems { get; set; }
        }

        [DataMember(Name = "supportedModalities")]
        public string[] SupportedModalities { get; set; }

        [DataMember(Name = "supportedMessageFormats")]
        public string[] SupportedMessageFormats { get; set; }

        [DataMember(Name = "_links")]
        public LinksObject Links { get; set; }

        [DataMember(Name = "rel")]
        public string Rel { get; set; }

        [DataMember(Name = "etag")]
        public string ETag { get; set; }

        [JsonExtensionData]
#pragma warning disable 0169
        private IDictionary<string, JToken> _extra_data;
#pragma warning restore 0169
    }
}
