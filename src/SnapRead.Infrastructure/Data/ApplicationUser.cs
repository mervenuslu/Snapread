using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnapRead.Infrastructure.Data
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime CreatedAt { get; set; }
    }
}
