using System.Runtime.Serialization;

namespace UCWA.NET.Resources
{
    [DataContract]
    public class OnlineMeetings : Resource
    {
        [DataContract]
        public class LinksObject
        {
            [DataMember(Name = "self")]
            public HrefObject Self { get; set; }

            [DataMember(Name = "myOnlineMeetings")]
            public HrefObject MyOnlineMeetings { get; set; }

            [DataMember(Name = "onlineMeetingDefaultValues")]
            public HrefObject OnlineMeetingDefaultValues { get; set; }

            [DataMember(Name = "onlineMeetingEligibleValues")]
            public HrefObject OnlineMeetingEligibleValues { get; set; }

            [DataMember(Name = "onlineMeetingInvitationCustomization")]
            public HrefObject OnlineMeetingInvitationCustomization { get; set; }

            [DataMember(Name = "onlineMeetingPolicies")]
            public HrefObject OnlineMeetingPolicies { get; set; }

            [DataMember(Name = "phoneDialInInformation")]
            public HrefObject PhoneDialInInformation { get; set; }

            [DataMember(Name = "myAssignedOnlineMeeting")]
            public HrefObject MyAssignedOnlineMeeting { get; set; }
        }

        [DataMember(Name = "_links")]
        public LinksObject Links { get; set; }

        [DataMember(Name = "rel")]
        public string Rel { get; set; }
    }
}
