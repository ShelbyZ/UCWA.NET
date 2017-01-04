using System.Runtime.Serialization;

namespace UCWA.NET.Resources
{
    [DataContract]
    public class HrefObject
    {
        [DataMember(Name = "href")]
        public string Href { get; set; }

        [DataMember(Name = "revision", IsRequired = false, EmitDefaultValue = false)]
        public string Revision { get; set; }
    }
}
