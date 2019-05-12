using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace smintbuster.Modals
{
    public class ApplicationCategory
    {
        [Column(TypeName = "nvarchar(150)")]
        public string Name { get; set; }
    }
}
