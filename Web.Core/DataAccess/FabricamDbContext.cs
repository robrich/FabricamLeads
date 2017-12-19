using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Fabricam.Web.Core.DataAccess
{
    public class FabricamDbContext : DbContext
    {
        public FabricamDbContext(DbContextOptions<FabricamDbContext> options)
            : base(options)
        {
        }

        // FRAGILE: the property name is the table name

        public DbSet<Lead> Lead { get; set; }
    }
}
