using System;
using System.Collections.Generic;

namespace WebApi10Min.Models
{
    public partial class LogBackend
    {
        public int IdLog { get; set; }
        public string User { get; set; }
        public string UserIp { get; set; }
        public DateTime? Datetime { get; set; }
        public string Message { get; set; }
        public string EventType { get; set; }
    }
}
