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
        public DbSet<Voucher> Voucher { get; set; }
        public DbSet<Movie> Movie { get; set; }
        public DbSet<Reservation> Reservation { get; set; }
        public DbSet<Guest> Guest { get; set; }
    }
}