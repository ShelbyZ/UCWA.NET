using System;
using UCWA.NET.Resources;

namespace UCWA.NET.Core
{
    public class EventReceivedEventArgs : EventArgs
    {
        public Resource Resource { get; set; }
    }
}
