using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApplication1.Models
{
    public partial class JWTContext : DbContext
    {
        public JWTContext()
        {
        }

        public JWTContext(DbContextOptions<JWTContext> options)
            : base(options)
        {
        }

        public DbSet<WebApplication1.Models.User> Users { get; set; }
      
    }
}
