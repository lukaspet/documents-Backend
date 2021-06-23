using System;
using System.Collections.Generic;

namespace WebApi10Min.Models
{
    public partial class LogFrontend
    {
        public int IdLog { get; set; }
        public string User { get; set; }
        public string UserIp { get; set; }
        public DateTime? DatetimeBackend { get; set; }
        public DateTime? DatetimeFrontend { get; set; }
        public string Message { get; set; }
        public string EventType { get; set; }
        public string Url { get; set; }
        public DateTime? DatetimeInsert { get; set; }
    }
}
