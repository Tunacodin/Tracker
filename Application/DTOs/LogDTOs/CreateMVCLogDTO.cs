using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DTOs.LogDTOs
{
    public class CreateMVCLogDTO
    {
        public string Page { get; set; }
        public string? TargetEmail { get; set; }
    }
}
