using System.Runtime.Serialization;

namespace UCWA.NET.Resources
{
    [DataContract]
    public class Error : Resource
    {
        [DataMember(Name = "code")]
        public string Code { get; set; }

        [DataMember(Name = "subcode")]
        public string Subcode { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }
    }
}
