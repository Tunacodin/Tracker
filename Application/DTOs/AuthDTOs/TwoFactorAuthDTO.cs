﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.AuthDTOs
{
    public class TwoFactorAuthDTO
    {
        public string Email { get; set; }
        public string VerificationCode { get; set; }
    }
}
