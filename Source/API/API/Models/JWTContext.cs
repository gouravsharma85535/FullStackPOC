using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace API.Models
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

        public DbSet<API.Models.User> Users { get; set; }
      
    }
}
