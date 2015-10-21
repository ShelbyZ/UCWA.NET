using System;
using UCWA.NET.Resources;

namespace UCWA.NET.Core
{
    public class ErrorEventArgs : EventArgs
    {
        public Error Error { get; set; }
    }
}
