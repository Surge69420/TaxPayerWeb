﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public byte[]? ImageData { get; set; }
    }
}
