using System;
using System.Collections.Generic;

namespace UCWA.NET.Resources
{
    public class Challenge
    {
        public Uri MsRtcOAuth { get; private set; }

        public List<string> GrantTypes { get; private set; }

        public Challenge(string header)
        {
            GrantTypes = new List<string>();
            var msRtcOAuthHref = "MsRtcOAuth href=";
            var grantType = "grant_type=";
            var index = header.IndexOf(msRtcOAuthHref, StringComparison.InvariantCultureIgnoreCase);

            if (index != -1)
            {
                var temp = header.Substring(index + msRtcOAuthHref.Length);
                index = temp.IndexOf('"', 1);
                temp = temp.Substring(0, index);
                MsRtcOAuth = new Uri(temp.Replace("\"", ""));
            }

            index = header.IndexOf(grantType, StringComparison.InvariantCultureIgnoreCase);

            if (index != -1)
            {
                var temp = header.Substring(index + grantType.Length);
                temp = temp.Replace("\"", "");
                foreach (var type in temp.Split(','))
                {
                    GrantTypes.Add(type);
                }
            }
        }
    }
}
