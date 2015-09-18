using System;
using System.Collections.Generic;

namespace UCWA.NET.Transport
{
    public class WebAction
    {
        public Uri Uri { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public byte[] Data { get; set; }

        public WebAction()
        {
            Headers = new Dictionary<string, string>();
        }
    }
}
