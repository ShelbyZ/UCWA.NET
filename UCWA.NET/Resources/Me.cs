using System.Runtime.Serialization;

namespace UCWA.NET.Resources
{
    [DataContract]
    public class Me : Resource
    {
        [DataContract]
        public class LinksObject
        {
            [DataMember(Name = "self")]
            public HrefObject Self { get; set; }

            [DataMember(Name = "makeMeAvailable")]
            public HrefObject MakeMeAvailable { get; set; }

            [DataMember(Name = "callForwardingSettings")]
            public HrefObject CallForwardingSettings { get; set; }

            [DataMember(Name = "phones")]
            public HrefObject Phones { get; set; }

            [DataMember(Name = "photo")]
            public HrefObject Photo { get; set; }
        }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "uri")]
        public string Uri { get; set; }

        [DataMember(Name = "emailAddresses")]
        public string[] EmailAddresses { get; set; }

        [DataMember(Name = "_links")]
        public LinksObject Links { get; set; }

        [DataMember(Name = "rel")]
        public string Rel { get; set; }
    }
}
