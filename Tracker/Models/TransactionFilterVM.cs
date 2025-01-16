using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tracker.Models
{
    public class TransactionFilterVM
    {
        public string Phone { get; set; }
        public int Count { get; set; }
        public string? FirstDate { get; set; }
        public string? LastDate { get; set; }
    }
}
