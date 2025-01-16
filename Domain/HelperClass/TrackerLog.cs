using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Enums;
using Domain.Interfaces;

namespace Domain.HelperClass
{
    public class TrackerLog : IBaseEntity
    {
        public string Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public Status Status { get; set; }
        public string Message { get; set; }
        public string? LoggedInUserEmail { get; set; }
        public string? TargetEmail { get; set; }
        public string? TargetAccount { get; set; }
        public string? IpAddress { get; set; }
        public ActionType? ActionType { get; set; }
        public string? Page { get; set; }
        public ProcessStatus? ProcessStatus { get; set; }
    }
}
