using System.Runtime.Serialization;

namespace UCWA.NET.Resources
{
    [DataContract]
    public class People : Resource
    {
        [DataContract]
        public class LinksObject
        {
            [DataMember(Name = "self")]
            public HrefObject Self { get; set; }

            [DataMember(Name = "myContacts")]
            public HrefObject MyContacts { get; set; }

            [DataMember(Name = "myContactsAndGroupsSubscription")]
            public HrefObject MyContactsAndGroupsSubscription { get; set; }

            [DataMember(Name = "myGroupMemberships")]
            public HrefObject MyGroupMemberships { get; set; }

            [DataMember(Name = "myGroups")]
            public HrefObject MyGroups { get; set; }

            [DataMember(Name = "myPrivacyRelationships")]
            public HrefObject MyPrivacyRelationships { get; set; }

            [DataMember(Name = "presenceSubscriptionMemberships")]
            public HrefObject PresenceSubscriptionMemberships { get; set; }

            [DataMember(Name = "presenceSubscriptions")]
            public HrefObject PresenceSubscriptions { get; set; }

            [DataMember(Name = "search")]
            public HrefObject Search { get; set; }

            [DataMember(Name = "subscribedContacts")]
            public HrefObject SubscribedContacts { get; set; }
        }

        [DataMember(Name = "_links")]
        public LinksObject Links { get; set; }

        [DataMember(Name = "rel")]
        public string Rel { get; set; }
    }
}
