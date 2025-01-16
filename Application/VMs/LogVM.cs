using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Enums;

namespace Application.VMs
{
    public class LogVM
    {
        public string Id { get; set; }
        public DateTime CreationDate { get; set; }
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
