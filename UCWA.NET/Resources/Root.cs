using System.Runtime.Serialization;

namespace UCWA.NET.Resources
{
    [DataContract]
    public class Root : Resource
    {
        [DataContract]
        public class LinksObject
        {
            [DataMember(Name = "self")]
            public HrefObject Self { get; set; }

            [DataMember(Name = "applications")]
            public HrefObject Applications { get; set; }

            [DataMember(Name = "xframe")]
            public HrefObject Xframe { get; set; }
        }

        [DataMember(Name = "_links")]
        public LinksObject Links { get; set; }
    }
}
