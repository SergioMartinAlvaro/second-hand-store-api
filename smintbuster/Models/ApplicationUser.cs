﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace smintbuster.Modals
{
    public class ApplicationUser : IdentityUser
    {
        [Column(TypeName = "nvarchar(150)")]
        public string NickName { get; set; }
        [Column(TypeName ="nvarchar(150)")]
        public string FirstName { get; set; }
        [Column(TypeName = "nvarchar(150)")]
        public string LastName { get; set; }
        [Column(TypeName = "nvarchar(150)")]
        public string UserType { get; set; }
        [Column(TypeName = "nvarchar(150)")]
        public string Email { get; set; }
    }
}
