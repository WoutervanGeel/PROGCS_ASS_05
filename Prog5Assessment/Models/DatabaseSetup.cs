using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Prog5Assessment.Models
{
    public class DatabaseSetup : DbContext
    {
        public DatabaseSetup() : base("MyConnectionString")
        {
            
        }

        public DbSet<Room> Room { get; set; }
        public DbSet<CustomPrices> CustomPrices { get; set; }

    }
}