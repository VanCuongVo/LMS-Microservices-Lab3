using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Application.DTOs.Request
{
    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; } = string.Empty;

    }
}