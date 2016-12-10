using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UCWA.NET.Resources
{
    [DataContract]
    public class Event : Resource
    {
        [DataContract]
        public class LinksObject
        {
            [DataMember(Name = "self")]
            public HrefObject Self { get; set; }

            [DataMember(Name = "next")]
            public HrefObject Next { get; set; }

            [DataMember(Name = "resync")]
            public HrefObject Resync { get; set; }
        }

        [DataContract]
        public class SenderObject
        {
            [DataMember(Name = "rel")]
            public string Rel { get; set; }

            [DataMember(Name = "href")]
            public string Href { get; set; }

            [DataMember(Name = "events")]
            public EventsObject[] Events { get; set; }
        }

        [DataContract]
        public class EventsObject
        {
            [DataMember(Name = "link")]
            public LinkObject Link { get; set; }

            [DataMember(Name = "_embedded")]
            public Dictionary<string, Dictionary<string, object>> Embedded { get; set; }

            [DataMember(Name = "type")]
            public string Type { get; set; }
        }

        [DataContract]
        public class LinkObject
        {
            [DataMember(Name = "rel")]
            public string Rel { get; set; }

            [DataMember(Name = "href")]
            public string Href { get; set; }
        }

        [DataMember(Name = "_links")]
        public LinksObject Links { get; set; }

        [DataMember(Name = "sender")]
        public SenderObject[] Sender { get; set; }
    }
}
