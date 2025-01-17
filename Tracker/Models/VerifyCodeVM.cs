using System.ComponentModel.DataAnnotations;

namespace Tracker.Models
{
    public class VerifyCodeVM
    {
        public string Email { get; set; }
        public string VerificationCode { get; set; }
    }
}