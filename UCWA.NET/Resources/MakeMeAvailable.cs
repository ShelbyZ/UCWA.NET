using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UCWA.NET.Resources
{
    [DataContract]
    public class MakeMeAvailable : Resource
    {
        [DataMember(Name = "phoneNumber", IsRequired = false, EmitDefaultValue = false)]
        public string PhoneNumber { get; set; }

        [DataMember(Name = "signInAs", IsRequired = false, EmitDefaultValue = false)]
        public string SignInAs { get; set; }

        [DataMember(Name = "supportedMessageFormats")]
        public List<string> SupportedMessageFormats { get; set; }

        [DataMember(Name = "supportedModalities")]
        public List<string> SupportedModalities { get; set; }

        public MakeMeAvailable()
        {
            SupportedMessageFormats = new List<string>();
            SupportedModalities = new List<string>();
        }
    }
}
