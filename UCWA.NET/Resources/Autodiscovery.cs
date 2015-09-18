using System.Runtime.Serialization;

namespace UCWA.NET.Resources
{
    [DataContract]
    public class Autodiscovery : Resource
    {
        [DataContract]
        public class LinksObject
        {
            [DataMember(Name = "self")]
            public HrefObject Self { get; set; }

            [DataMember(Name = "user")]
            public HrefObject User { get; set; }

            [DataMember(Name = "xframe")]
            public HrefObject Xframe { get; set; }
        }

        [DataMember(Name = "_links")]
        public LinksObject Links { get; set; }
    }
}
