namespace UCWA.NET.Resources
{
    public static class Constants
    {
        // Headers
        public readonly static string Authorization = "Authorization";
        public readonly static string ContentType = "Content-Type";
        public readonly static string IfMatch = "If-Match";
        public readonly static string WwwAuthenticate = "WWW-Authenticate";

        // Content & Accept types
        public readonly static string Json = "application/json";
        public readonly static string XWwwFormUrlencoded = "application/x-www-form-urlencoded;charset=UTF-8";

        // grant_types
        public readonly static string Password = "password";
        public readonly static string Windows = "urn:microsoft.rtc:windows";
        public readonly static string Passive = "urn:microsoft.rtc:passive";
        public readonly static string AnonMeeting = "urn:microsoft.rtc:anonmeeting";
    }
}
