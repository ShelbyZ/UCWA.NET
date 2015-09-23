using System.Runtime.Serialization;

namespace UCWA.NET.Resources
{
    [DataContract]
    public class AuthToken : Resource
    {
        [DataMember(Name = "access_token")]
        public string AccessToken { get; set; }

        [DataMember(Name = "expires_in")]
        public int ExpiresIn { get; set; }

        [DataMember(Name = "ms_rtc_identityscope")]
        public string MsRtcIdentityScope { get; set; }

        [DataMember(Name = "token_type")]
        public string TokenType { get; set; }
    }
}
