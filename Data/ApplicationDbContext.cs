using System;
using System.Collections.Generic;
using System.Text;
using alldux_plataforma.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace alldux_plataforma.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)     : base(options)
        {
            
        }
    }
}

