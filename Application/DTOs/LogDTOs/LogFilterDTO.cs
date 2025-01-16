using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Enums;

namespace Application.DTOs.LogDTOs
{
    public class LogFilterDTO : PaginationDTO
    {
        public string? TargetAccount { get; set; }
        public string? TargetEmail { get; set; }
        public string? LoggedInUserEmail { get; set; }
        public string? Page { get; set; }
        public ActionType? ActionType { get; set; }
        public ProcessStatus? ProcessStatus { get; set; }
    }
}
